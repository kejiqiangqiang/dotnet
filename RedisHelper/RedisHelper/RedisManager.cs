using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    public class RedisManager
    {
        /// <summary>
        /// 默认连接字符串
        /// </summary>
        private static readonly string ConnectionString;
        /// <summary>
        /// redis 连接单例对象
        /// </summary>
        private static IConnectionMultiplexer _connectionMultiplexer;
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        ///// <summary>
        ///// 默认的 Key 值（用来当作 RedisKey 的前缀）
        ///// </summary>
        //private static readonly string DefaultKey;

        ///// <summary>
        ///// 数据库
        ///// </summary>
        //private readonly IDatabase _db;

        static RedisManager()
        {
            ConnectionString = "127.0.0.1:6379";
            ConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectionString"]?.ConnectionString;
            //DefaultKey = ConfigurationManager.AppSettings["Redis.DefaultKey"];
        }

        /// <summary>
        /// 获取单例对象（使用默认连接）
        /// </summary>
        /// <returns></returns>
        public static IConnectionMultiplexer ConnectionInstance
        {
            get
            {
                if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                {
                    lock (Locker)
                    {
                        if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                        {
                            _connectionMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                            RedisManager.AddRegisterEvent(_connectionMultiplexer);
                        }
                    }
                }
                return _connectionMultiplexer;
            }
        }
        
        /// <summary>
        /// 获取默认连接数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int db=0)
        {
            return RedisManager.ConnectionInstance.GetDatabase(db);
        }

        ///Dictionary非线程安全，需要加锁

        /// <summary>
        /// redis 连接对象缓存集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, IConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, IConnectionMultiplexer>();
        /// <summary>
        /// redis 连接对象内的数据库缓存集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, IDatabase> DbCache = new ConcurrentDictionary<string, IDatabase>();


        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            connectionString = connectionString ?? ConnectionString;
            bool isExist = ConnectionCache.TryGetValue(connectionString,out IConnectionMultiplexer connMultiplexer);
            if (!isExist||!connMultiplexer.IsConnected)
            {
                connMultiplexer = GetConnectionInatance(connectionString);
                ConnectionCache.TryAdd(connectionString, connMultiplexer);
            }
            return connMultiplexer;
        }

        /// <summary>
        /// 获取连接对象实例
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IConnectionMultiplexer GetConnectionInatance(string connectionString)
        {
            connectionString = connectionString ?? ConnectionString;
            IConnectionMultiplexer conn = ConnectionMultiplexer.Connect(connectionString);
            AddRegisterEvent(conn);
            return conn;
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IDatabase GetDatabase(string connectionString,int db)
        {
            connectionString = connectionString ?? ConnectionString;
            string key = $"{connectionString}/{db}";
            bool isExist = DbCache.TryGetValue(key,out IDatabase database);
            if (!isExist)
            {
                database = GetConnectionMultiplexer(connectionString).GetDatabase(db);
                DbCache.TryAdd(key,database);
            }
            return database;
        }

        #region 注册事件

        private static void AddRegisterEvent(IConnectionMultiplexer connMultiplexer)
        {
            connMultiplexer.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            connMultiplexer.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            connMultiplexer.ErrorMessage += ConnMultiplexer_ErrorMessage;
            connMultiplexer.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            connMultiplexer.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            connMultiplexer.InternalError += ConnMultiplexer_InternalError;
            connMultiplexer.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }
        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender,EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}:{e.EndPoint}");
        }
        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_InternalError)}:{e.Exception}");
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine(
                $"{nameof(ConnMultiplexer_HashSlotMoved)}:{nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint}, ");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChanged)}:{e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ErrorMessage)}:{e.Message}");
        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionFailed)}:{e.Exception}");
        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }

        #endregion


    }

    #region Demo
    ///// <summary>
    ///// ConnectionMultiplexer对象管理帮助类
    ///// </summary>
    //public static class RedisConnectionHelp {
    //    //系统自定义Key前缀
    //    public static readonly string SysCustomKey = ConfigurationManager.AppSettings["redisKey"] ?? ""; 
    //    //"127.0.0.1:6379,allowadmin=true 
    //    private static readonly string RedisConnectionString = ConfigurationManager.ConnectionStrings["RedisExchangeHosts"].ConnectionString;
    //    private static readonly object Locker = new object();
    //    private static ConnectionMultiplexer _instance;
    //    private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>(); 
    //    /// <summary>
    //    /// 单例获取
    //    /// </summary>
    //    public static ConnectionMultiplexer Instance {
    //        get {
    //            if (_instance == null) {
    //                lock (Locker) {
    //                    if (_instance == null || !_instance.IsConnected) {
    //                        _instance = GetManager();
    //                    }
    //                }
    //            } return _instance;
    //        }
    //    } 
    //    /// <summary>
    //    /// 缓存获取
    //    /// </summary>
    //    /// <param name="connectionString"></param>
    //    /// <returns></returns> 
    //    public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
    //    {
    //        if (!ConnectionCache.ContainsKey(connectionString)) {
    //            ConnectionCache[connectionString] = GetManager(connectionString);
    //        }
    //        return ConnectionCache[connectionString];
    //    }
    //    private static ConnectionMultiplexer GetManager(string connectionString = null) {
    //        connectionString = connectionString ?? RedisConnectionString;
    //        var connect = ConnectionMultiplexer.Connect(connectionString);
    //        //注册如下事件 
    //        connect.ConnectionFailed += MuxerConnectionFailed;
    //        connect.ConnectionRestored += MuxerConnectionRestored;
    //        connect.ErrorMessage += MuxerErrorMessage;
    //        connect.ConfigurationChanged += MuxerConfigurationChanged;
    //        connect.HashSlotMoved += MuxerHashSlotMoved;
    //        connect.InternalError += MuxerInternalError;
    //        return connect;
    //    } 

    //    #region 事件 
    //    /// <summary> 
    //    /// 配置更改时
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param> 
    //    private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e) {
    //        Console.WriteLine("Configuration changed: " + e.EndPoint);
    //    } 
    //    /// <summary>
    //    /// 发生错误时
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param> 
    //    private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e) {
    //        Console.WriteLine("ErrorMessage: " + e.Message);
    //    } 
    //    /// <summary> /// 
    //    /// 重新建立连接之前的错误 /// 
    //    /// </summary> /// 
    //    /// <param name="sender"></param> /// 
    //    /// <param name="e"></param> 
    //    private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e) {
    //        Console.WriteLine("ConnectionRestored: " + e.EndPoint);
    //    } 
    //    /// <summary> /// 
    //    /// 连接失败 ， 如果重新连接成功你将不会收到这个通知 ///
    //    /// </summary> ///
    //    /// <param name="sender"></param> /// 
    //    /// <param name="e"></param> 
    //    private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e) {
    //        Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
    //    } 
    //    /// <summary> /// 更改集群 
    //    /// /// </summary> /// 
    //    /// <param name="sender"></param> /// 
    //    /// <param name="e"></param> 
    //    private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e) {
    //        Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
    //    } 
    //    /// <summary> /// 
    //    /// redis类库错误 /// 
    //    /// </summary> /// 
    //    /// <param name="sender"></param> /// 
    //    /// <param name="e"></param> 
    //    private static void MuxerInternalError(object sender, InternalErrorEventArgs e) { Console.WriteLine("InternalError:Message" + e.Exception.Message);
    //    }
    //    #endregion 事件 
    //}
    #endregion

}
