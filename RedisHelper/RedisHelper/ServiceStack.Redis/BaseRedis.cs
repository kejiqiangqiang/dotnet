using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace Redis
{
    /// <summary>
    /// Redis数据访问
    /// </summary>
    public class BaseRedis
    {
        /// <summary>
        /// 数据库缓存
        /// </summary>
        /// <param name="redisPath">Redis路径</param>
        public BaseRedis(string redisPath)
        {
            this.redisPath = redisPath;
        }
        /// <summary>
        /// 数据库缓存
        /// </summary>
        public BaseRedis()
        {

        }

        /// <summary>
        /// Redis路径
        /// </summary>
        private string redisPath;

        /// <summary>
        /// Redis路径
        /// </summary>
        public string RedisPath
        {
            get { return redisPath; }
            set { redisPath = value; }
        }


        /// <summary>
        /// 获取Redis连接
        /// </summary>
        private RedisClient GetRedisClient()
        {
            RedisClient redisClient = RedisManager.GetClient(this.redisPath);
            return redisClient;
        }


        /// <summary>
        /// 获取数据Key
        /// </summary>
        /// <param name="patternKey">关键</param>
        /// <returns>数据Key</returns>
        public List<T> SearchByPattern<T>(string patternKey)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                List<string> keys = RedisClient.SearchKeys(patternKey);
                var models = new List<T>();
                if (keys == null || keys.Count == 0)
                {
                    return models;
                }
                var val = RedisClient.GetAll<T>(keys);
                foreach (var key in val.Keys)
                {
                    if (val[key] != null)
                    {
                        models.Add(val[key]);
                    }
                }
                return models;
            }
        }

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public bool Set<T>(string key, T value)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var isOk = RedisClient.Set<T>(key, value);
                return isOk;
            }
        }

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns>是否成功</returns>
        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var isOk = RedisClient.Set<T>(key, value, expiresAt);
                return isOk;
            }
        }

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns>是否成功</returns>
        public bool Set<T>(string key, T value, TimeSpan timeSpan)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var isOk = RedisClient.Set<T>(key, value, timeSpan);
                return isOk;
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据</returns>
        public T Get<T>(string key) where T : class
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var val = RedisClient.Get<T>(key);
                return val;
            }
        }

        /// <summary>
        /// 获取多个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="keys">键</param>
        /// <returns>数据</returns>
        public IDictionary<string, T> GetAll<T>(List<string> keys)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                if (keys == null || keys.Count == 0)
                {
                    return new Dictionary<string, T>();
                }
                var val = RedisClient.GetAll<T>(keys);
                return val;
            }
        }

        /// <summary>
        /// 设置多个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="values">数据</param>
        /// <returns>数据</returns>
        public bool SetAll<T>(IDictionary<string, T> values)
        {
            if (values == null || values.Count == 0)
            {
                return true;
            }
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                RedisClient.SetAll<T>(values);
                return true;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">数据Key</param>
        public bool Remove(string key)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var val = RedisClient.Remove(key);
                return val;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keys">数据Key</param>
        public void RemoveAll(List<string> keys)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                RedisClient.RemoveAll(keys);
            }
        }

        /// <summary>
        /// 根据模糊匹配来删除数据
        /// </summary>
        /// <param name="pattern">pattern Key</param>
        public bool RemoveByPattern(string pattern)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                List<string> keys = RedisClient.SearchKeys(pattern);
                foreach (var key in keys)
                {
                    RedisClient.Remove(key);
                }
                return true;
            }
        }

        #region 设置Hash数据
        /// <summary>
        /// 设置对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="hashId">hash编号</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public bool HSet<T>(string hashId, Dictionary<string, T> value) where T : class
        {
            if (value == null || value.Count == 0)
            {
                return true;
            }
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                Dictionary<string, string> nnn = new Dictionary<string, string>();
                int i = 0;
                foreach (var item in value)
                {
                    nnn.Add(hashId + ":" + item.Key, JsonConvert.SerializeObject(item.Value));
                    i++;
                }
                RedisClient.SetRangeInHash(hashId, nnn);
                return true;
            }
        }

        /// <summary>
        /// 获取Key
        /// </summary>
        /// <param name="hashId">hash编号</param>
        /// <returns>key</returns>
        public List<string> HGetKey(string hashId)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var keys=RedisClient.GetHashKeys(hashId);
                return keys;
            }
        }

        /// <summary>
        /// 根据主键获Hash对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="hashId">hash编号</param>
        /// <param name="keys">keys</param>
        /// <returns>数据</returns>
        public List<T> HGet<T>(string hashId, params string[] keys) where T : class
        {
            List<T> value = new List<T>();
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                List<string> rs = new List<string>();
                if (keys != null && keys.Length > 0)
                {
                    List<string> newKeys = keys.Select(p => hashId + ":" + p).ToList();
                    rs = RedisClient.GetValuesFromHash(hashId, newKeys.ToArray());
                }
                else
                {
                    rs = RedisClient.GetHashValues(hashId);
                    if (rs == null) { rs = new List<string>(); }
                }
                foreach (var item in rs)
                {
                    if (item != null)
                    {
                        value.Add(JsonConvert.DeserializeObject<T>(item));
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="hashId">hash编号</param>
        public bool HRemove(string hashId)
        {
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                var keys = RedisClient.GetHashKeys(hashId);
                foreach (var key in keys)
                {
                    RedisClient.RemoveEntryFromHash(hashId, key);
                }
            }
            return true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="hashId">hash编号</param>
        /// <param name="keys">数据Key</param>
        public void HRemoveAll(string hashId,List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return;
            }
            using (RedisClient RedisClient = this.GetRedisClient())
            {
                foreach (var key in keys)
                {
                    string newKey = key;
                    if (!newKey.Contains(hashId+":"))
                    {
                        newKey = hashId + ":" + newKey;
                    }
                    RedisClient.RemoveEntryFromHash(hashId, newKey);
                }
            }
        }

        #endregion
    }
}