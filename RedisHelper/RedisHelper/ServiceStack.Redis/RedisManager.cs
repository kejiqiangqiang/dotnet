using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Threading;

namespace Redis
{
    /// <summary>
    /// Redis客户端
    /// </summary>
    internal static class RedisManager
    {
        private static Mutex mutex = new Mutex();
        private static Dictionary<string, PooledRedisClientManager> Managers = new Dictionary<string, PooledRedisClientManager>();

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static PooledRedisClientManager CreateManager(string redisUrl)
        {
            var str = redisUrl.Split("/".ToCharArray());
            string redisPath = str[0];
            int defaultDb = 0;
            try
            {
                if (str.Length > 1)
                {
                    int.TryParse(str[1], out defaultDb);
                }
            }
            catch
            {
                throw new Exception("Redis连接字符串错误。" + redisUrl);
            }
            if (Managers.ContainsKey(redisUrl))
            {
                return Managers[redisUrl];
            }
            else
            {
                PooledRedisClientManager _prcm = new PooledRedisClientManager(new string[] { redisPath }, new string[] { redisPath }, new RedisClientManagerConfig
                {
                    MaxWritePoolSize = 20, // “写”链接池链接数 
                    MaxReadPoolSize = 20, // “读”链接池链接数 
                    AutoStart = true,
                    DefaultDb = defaultDb
                });
                Managers.Add(redisUrl, _prcm);
                return _prcm;
            }
        }
        /// <summary>
        /// 从连接池中获取连接对象
        /// </summary>
        public static RedisClient GetClient(string redisUrl)
        {
            try
            {
                mutex.WaitOne();
                PooledRedisClientManager mg = CreateManager(redisUrl);
                RedisClient client = mg.GetClient() as RedisClient;
                return client;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 将连接池的对象还回去
        /// </summary>
        public static void DisposeClient(string redisUrl, RedisClient client)
        {
            try
            {
                mutex.WaitOne();
                if (Managers.ContainsKey(redisUrl))
                {
                    Managers[redisUrl].DisposeClient(client);
                }
                else
                {
                    client.Dispose();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}