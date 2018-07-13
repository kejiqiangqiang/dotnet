using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProxy
{
    /// <summary>
    /// 被拦截的业务基类
    /// </summary>
    [BusinessClass]
    public abstract partial class AbsBusiness : ContextBoundObject//转换为远程对象(远程代理)MarshalByRefObject必不可少的
    {

        /// <summary>
        /// 数据库上下文DataContext
        /// </summary>
        private SqlDataContext _DataContext = null;




        /*-------------业务基类方法-------------*/

        /// <summary>
        /// 读取ConnectionString
        /// </summary>
        /// <returns>ConnectionString</returns>
        abstract public string ReaderConnectionString();

        /// <summary>
        /// 写日志记录
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="description">方法描述</param>
        abstract public void WriteLog(IMessage message, string description);

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="e">异常信息</param>
        abstract public void WriteException(IMessage message, Exception e);

        /// <summary>
        /// 数据库数据变化的表名用作Redis消息以同步数据库表数据到Redis
        /// </summary>
        /// <param name="tableNames"></param>
        abstract public void ChangeDataTable(List<string> tableNames);





        /*-------------LINQ to SQL框架入口System.Data.Linq.DataContext初始化(初始化数据库连接、打开连接、事务开启等)-------------*/

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        private SqlDataContext GetDataContext(BusinessMethodAttribute attr)
        {
            SqlDataContext dataContext = new SqlDataContext(this.ReaderConnectionString(),new DynamicMappingSource());
            dataContext.ObjectTrackingEnabled = false;
            dataContext.Log = Console.Out;
            dataContext.Connection.Open();
            DbTransaction trans = null;
            var attrTrans = attr.Transaction;
            if (attrTrans!=Transaction.None)
            {
                trans = attrTrans == Transaction.Lock ? dataContext.Connection.BeginTransaction() : attrTrans == Transaction.WithNoLock ? dataContext.Connection.BeginTransaction(IsolationLevel.ReadUncommitted) : dataContext.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
            }
            dataContext.Transaction=trans;
            //
            dataContext.DataTableChanged.Clear();

            return dataContext;
        }





        /*----------------Aop拦截调用方法---------------*/

        /// <summary>
        /// 构造方法开始
        /// </summary> 
        internal void ConstructionBegin(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr)
        {
            //若要支持在业务基类的子类即业务类的构造方法中访问数据库，则需要在Aop拦截构造方法开始，调用此处进行初始化数据库上下文(同MethodBegin方法实现)
        }

        /// <summary>
        /// 构造方法结束
        /// </summary> 
        internal void ConstructionEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr)
        {

        }

        /// <summary>
        /// 方法调用开始--注入SqlDataContext对象
        /// </summary> 
        internal void MethodBegin(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr)
        {
            //注入SqlDataContext对象到业务基类
            this._DataContext = this.GetDataContext(attr);
            //注入dao到业务基类--用注入的SqlDataContext对象，反射初始化业务子类方法中字段或属性的dao
            Type type = this.GetType();
            BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fi = type.GetFields(bf).Where(p => p.FieldType.GetInterface("BusinessProxy.ITemplateBaseDao") != null).ToArray();
            PropertyInfo[] pi = type.GetProperties().Where(p => p.PropertyType.GetInterface("BusinessProxy.ITemplateBaseDao") != null).ToArray();
            //字段中的dao
            foreach (var item in fi)
            {
                ITemplateBaseDao dao = item.GetValue(this) as ITemplateBaseDao;
                if (dao == null)
                {
                    dao = Activator.CreateInstance(item.FieldType) as ITemplateBaseDao;
                    item.SetValue(this, dao);
                }
                if (dao != null)
                {
                    dao.DataContext = this._DataContext;
                }

            }
            //属性中的dao
            foreach (var item in pi)
            {
                ITemplateBaseDao dao = item.GetValue(this, null) as ITemplateBaseDao;
                if (dao==null)
                {
                    dao = Activator.CreateInstance(item.PropertyType) as ITemplateBaseDao;
                    item.SetValue(this, dao);
                }
                if (dao!=null)
                {
                    dao.DataContext = this._DataContext;
                }
            }
        }

        /// <summary>
        /// 方法调用结束--释放上下文资源
        /// </summary> 
        internal void MethodEnd(IMessage msg, IMethodReturnMessage methodReturnMsg, BusinessMethodAttribute attr)
        {
            if (this._DataContext==null)
            {
                return;
            }
            List<string> tableNames = this._DataContext.DataTableChanged;
            bool isChanged = true;
            try
            {
                if ((attr.Transaction==Transaction.Lock||attr.Transaction==Transaction.WithNoLock)&&methodReturnMsg.Exception==null)
                {
                    this._DataContext.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                if (attr.Transaction==Transaction.Lock||attr.Transaction==Transaction.WithNoLock)
                {
                    this._DataContext.Transaction.Rollback();
                }
                isChanged = false;
            }
            finally
            {
                //回收资源
                if (this._DataContext.Transaction!=null)
                {
                    this._DataContext.Transaction.Dispose();
                }
                if (this._DataContext.Connection!=null&&this._DataContext.Connection.State!=ConnectionState.Closed)
                {
                    this._DataContext.Connection.Close();
                    this._DataContext.Connection.Dispose();
                }
                this._DataContext.Dispose();
                this._DataContext = null;
            }
            if (methodReturnMsg.Exception!=null)
            {
                if (attr.LogOption&&!string.IsNullOrEmpty(attr.Description))
                {
                    this.WriteLog(msg,attr.Description);
                }
            }
            else
            {
                this.WriteException(msg,methodReturnMsg.Exception);
            }

            if (isChanged)
            {
                this.ChangeDataTable(tableNames);
            }

        }






    }
}
