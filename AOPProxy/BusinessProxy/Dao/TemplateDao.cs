using System.Reflection;
using System.Data.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using BusinessProxy.LinqToSQL.Extend.Core.Linq;
using System.Xml;
using System.IO;
using System.Text;
namespace BusinessProxy
{
    /// <summary>
    /// 数据访问类抽象基类--执行LINQ to SQL架构数据访问
    /// </summary>
    public abstract class TemplateDao<T> : BaseTemplateDao where T : class
    {
        /// <summary>
        /// 插入自动增长列开关
        /// </summary>
        private bool identity_insert = false;
        
        public Table<T> Entity
        {
            get
            {
                if (this.DataContext == null)
                {
                    throw new Exception("业务方法尚未标记:BusinessMethod属性");
                }
                return this.DataContext.GetTable<T>();
            }
        }

        /// <summary>
        /// 设置插入自动增长列开启
        /// </summary>
        /// <returns></returns>
        public void IDENTITY_INSERT_ON()
        {
            this.identity_insert = true;
            Table<T> table = this.DataContext.GetTable<T>();
            table.SetIdentityInsert(this.identity_insert);
        }

        /// <summary>
        /// 设置插入自动增长列关闭
        /// </summary>
        /// <returns></returns>
        public void IDENTITY_INSERT_OFF()
        {
            this.identity_insert = false;
            Table<T> table = this.DataContext.GetTable<T>();
            table.SetIdentityInsert(this.identity_insert);
        }

        /// <summary>
        /// 记录当前数据变化的表
        /// </summary>
        public void SetChangeTableName()
        {
            var metaTable = this.DataContext.Mapping.GetTable(typeof(T));
            string tableName = metaTable.TableName;
            this.DataContext.DataTableChanged.Add(tableName);
            this.DataContext.DataTableChanged = this.DataContext.DataTableChanged.Distinct().ToList();
        }

        #region Linq对数据库更新操作

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="model">实体</param>
        public virtual void Add(T model)
        {
            Table<T> table = this.DataContext.GetTable<T>();
            table.Add(identity_insert, model);
            this.SetChangeTableName();
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="models">实体</param>
        public virtual bool Add(List<T> models)
        {
            Table<T> table = this.DataContext.GetTable<T>();
            foreach (T model in models)
            {
                table.Add(identity_insert, model);
            }
            if (models.Count > 0)
            {
                this.SetChangeTableName();
            }
            return true;
        }

        /// <summary>
        /// 单表删除数据
        /// </summary>
        /// <param name="expression">Expression表达式</param>
        /// <returns>影响的行数</returns>
        public virtual int Delete(Expression<Func<T, bool>> expression)
        {
            Table<T> table = this.DataContext.GetTable<T>();
            this.SetChangeTableName();
            return table.Delete(expression);
        }

        /// <summary>
        /// 删除全部数据(需慎用此方法)
        /// </summary>
        public virtual void DeleteAll()
        {
            Table<T> table = this.DataContext.GetTable<T>();
            this.SetChangeTableName();
            table.DeleteAll();
        }

        /// <summary>
        /// 截断表数据(需慎用此方法)
        /// </summary>
        public virtual void Truncate()
        {
            Table<T> table = this.DataContext.GetTable<T>();
            this.SetChangeTableName();
            table.Truncate();
        }

        /// <summary>
        /// 级联删除数据
        /// </summary>
        /// <param name="expression">Expression表达式</param>
        /// <returns>影响的行数</returns>
        public virtual int DeleteBatch(Expression<Func<T, bool>> expression)
        {
            Table<T> table = this.DataContext.GetTable<T>();
            this.SetChangeTableName();
            return table.DeleteBatch(expression);
        }

        /// <summary>
        /// 单表更新数据
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="updater">更新表达式</param>
        /// <returns></returns>
        public virtual int UpdateWhere(Expression<Func<T, bool>> expression, Expression<Func<T, T>> updater)
        {
            Table<T> table = this.Entity;
            this.SetChangeTableName();
            return table.UpdateWhere(expression, updater);
        }

        /// <summary>
        /// 单表更新数据
        /// </summary>
        /// <param name="model">实体</param> 
        public virtual int Update(T model)
        {
            Table<T> table = this.Entity;
            this.SetChangeTableName();
            return table.Update(model);
        }

        /// <summary>
        /// 单表更新数据
        /// </summary>
        /// <param name="models">实体</param> 
        public virtual bool Update(List<T> models)
        {
            Table<T> table = this.DataContext.GetTable<T>();
            foreach (T model in models)
            {
                table.Update(model);
            }
            if (models.Count > 0)
            {
                this.SetChangeTableName();
            }
            return true;
        }

        /// <summary>
        /// 级联更新数据
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="updater">更新表达式</param>
        /// <returns>受影响的行数</returns>
        public virtual int UpdateBatch(Expression<Func<T, bool>> expression, Expression<Func<T, T>> updater)
        {
            Table<T> table = this.Entity;
            this.SetChangeTableName();
            return table.UpdateBatch(table.Where(expression), updater);
        }

        /// <summary>
        /// 使用SqlBulkCopy插入数据
        /// </summary>
        /// <param name="models">实体对象</param>
        public bool BulkCopy(List<T> models)
        {
            Table<T> table = this.Entity;
            table.BulkCopy<T>(models);
            this.SetChangeTableName();
            return true;
        }

        /// <summary>
        /// 使用SqlBulkCopy插入数据
        /// </summary>
        /// <param name="table">数据</param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public bool BulkCopy(DataTable table, string tableName)
        {
            table.BulkCopy(tableName, this.DataContext);
            this.SetChangeTableName();
            return true;
        }

        #endregion


        /// <summary>
        /// Linq2SQL单表分页
        /// </summary>
        /// <param name="count">行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据</returns>
        public abstract IQueryable<T> LoadAllWithPage(out int count, int pageIndex, int pageSize);



    }
}