using System;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections;
using FastLambda;

namespace BusinessProxy.LinqToSQL.Extend.Core.Linq
{
    public static partial class LinqToSqlExtensions
    {
        /// <summary>
        /// 删除数据
        /// </summary> 
        public static int Delete<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            //查询条件表达式转换成SQL的条件语句
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(filter.Body, cmd);

            string commandWhere = cmd.CommandText;
            //获取表名
            string tableName = "[" + table.Context.Mapping.GetTable(typeof(TEntity)).TableName + "]";
            //SQL命令
            string commandText = string.Format("DELETE FROM {0} WHERE {1}", tableName, commandWhere);
            cmd.CommandText = commandText;
            if (table.Context.Connection.State != ConnectionState.Open)
            {
                table.Context.Connection.Open();
            }
            if (table.Context.Transaction != null)
            {
                cmd.Transaction = table.Context.Transaction;
            }
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 截断表数据
        /// </summary> 
        public static int Truncate<TEntity>(this Table<TEntity> table) where TEntity : class
        {
            string tableName = "[" + table.Context.Mapping.GetTable(typeof(TEntity)).TableName + "]";
            string commandText = "truncate table " + tableName;
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            cmd.CommandText = commandText;
            if (table.Context.Connection.State != ConnectionState.Open)
            {
                table.Context.Connection.Open();
            }
            if (table.Context.Transaction != null)
            {
                cmd.Transaction = table.Context.Transaction;
            }
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 删除全部数据
        /// </summary> 
        public static int DeleteAll<TEntity>(this Table<TEntity> table) where TEntity : class
        {
            string tableName = "[" + table.Context.Mapping.GetTable(typeof(TEntity)).TableName + "]";
            string commandText = "delete  " + tableName;
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            if (table.Context.Connection.State != ConnectionState.Open)
            {
                table.Context.Connection.Open();
            }
            if (table.Context.Transaction != null)
            {
                cmd.Transaction = table.Context.Transaction;
            }
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新数据使用Extensions更新
        /// </summary> 
        public static int UpdateWhere<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
        {
            DbCommand update = table.GetUpdateCommand<TEntity>(filter, evaluator);
            update.CommandType = CommandType.Text;
            DbConnection Connection = table.Context.Connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            update.Connection = Connection;
            update.Transaction = table.Context.Transaction;
            return update.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新数据普通的更新:将更新整个数据库字段
        /// </summary> 
        public static int Update<TEntity>(this Table<TEntity> table, TEntity entity) where TEntity : class
        {
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();

            Type entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            ReadOnlyCollection<MetaDataMember> dataMembers = metaTable.RowType.DataMembers;

            StringBuilder SET = new StringBuilder();
            StringBuilder Where = new StringBuilder();
            var TableName = "[" + metaTable.TableName + "]";

            foreach (MetaDataMember mm in dataMembers)
            {
                string MappedName = mm.MappedName;
                string Name = mm.MappedName;
                if (string.IsNullOrEmpty(MappedName))
                {
                    break;
                }
                if (!mm.IsDbGenerated && !mm.IsPrimaryKey)
                {
                    SET.Append(" [" + MappedName + "]=@" + MappedName + ",");
                    DbParameter ps = cmd.CreateParameter();
                    ps.ParameterName = MappedName;
                    ps.Value = entityType.GetProperty(mm.Name).GetValue(entity, null);
                    if (ps.Value == null)
                    {
                        ps.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(ps);
                }
                if (mm.IsPrimaryKey)
                {
                    Where.Append(" [" + MappedName + "]=@" + MappedName + " and");
                    DbParameter ps = cmd.CreateParameter();
                    ps.ParameterName = MappedName;
                    ps.Value = entityType.GetProperty(mm.Name).GetValue(entity, null);
                    cmd.Parameters.Add(ps);
                }
            }
            SET.Remove(SET.Length - 1, 1);
            Where.Remove(Where.Length - 3, 3);
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            string CommandText = " Update " + TableName + "\n SET" + SET + "\n Where" + Where;
            cmd.CommandText = CommandText;
            cmd.Transaction = table.Context.Transaction;
            int n = cmd.ExecuteNonQuery();
            return n;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="table">Table</param>
        /// <param name="entities">条件</param>
        /// <param name="identity_insert">是否插入自动增长列</param>
        /// <param name="evaluator">更新字段</param>
        /// <returns>n</returns>
        public static int Add<TEntity>(this Table<TEntity> table, bool identity_insert, TEntity entity) where TEntity : class
        {
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();

            Type entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            var tableName = "[" + metaTable.TableName + "]";
            string CommandText = "";

            ReadOnlyCollection<MetaDataMember> dataMembers = metaTable.RowType.DataMembers;
            List<object> values = new List<object>();

            //自动增长列
            string DbGeneratedColumnName = null;
            StringBuilder sbColumnNames = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            foreach (MetaDataMember mm in dataMembers)
            {
                if (mm.IsDbGenerated)
                {
                    DbGeneratedColumnName = mm.Name;
                }
            }

            //没有自动增长列或者有自动增长列并且要求按原列值插入的自动增长列的
            if (DbGeneratedColumnName == null || (DbGeneratedColumnName != null && identity_insert))
            {
                //构建列和参数
                foreach (MetaDataMember mm in dataMembers)
                {
                    if (mm.DbType != null)
                    {
                        sbColumnNames.Append("[" + mm.MappedName + "],");
                        sbValues.Append("@" + mm.MappedName + ",");
                        DbParameter ps = cmd.CreateParameter();
                        ps.ParameterName = mm.MappedName;
                        ps.Value = entityType.GetProperty(mm.Name).GetValue(entity, null);
                        if (ps.Value == null)
                        {
                            ps.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(ps);
                    }
                }
                //插入数据
                sbColumnNames.Remove(sbColumnNames.Length - 1, 1);
                sbValues.Remove(sbValues.Length - 1, 1);
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                CommandText = "INSERT INTO " + tableName + "(" + sbColumnNames + ") VALUES(" + sbValues + "); ";
                cmd.CommandText = CommandText;
                cmd.Transaction = table.Context.Transaction;
                int n = cmd.ExecuteNonQuery();
                return n;
            }
            else
            {
                foreach (MetaDataMember mm in dataMembers)
                {
                    if (!mm.IsDbGenerated && mm.DbType != null)
                    {
                        sbColumnNames.Append("[" + mm.MappedName + "],");
                        sbValues.Append("@" + mm.MappedName + ",");
                        DbParameter ps = cmd.CreateParameter();
                        ps.ParameterName = mm.MappedName;
                        ps.Value = entityType.GetProperty(mm.Name).GetValue(entity, null);
                        if (ps.Value == null)
                        {
                            ps.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(ps);
                    }
                }
                sbColumnNames.Remove(sbColumnNames.Length - 1, 1);
                sbValues.Remove(sbValues.Length - 1, 1);
                CommandText = "INSERT INTO " + tableName + "(" + sbColumnNames + ") VALUES(" + sbValues + "); SELECT CONVERT(int,SCOPE_IDENTITY()) AS [value]";
                cmd.CommandText = CommandText;
                cmd.Transaction = table.Context.Transaction;
                object n = cmd.ExecuteScalar();
                entityType.GetProperty(DbGeneratedColumnName).SetValue(entity, n, null);
                return 1;
            }
        }

        /// <summary>
        /// 设置是否能直接插入自动增长列的值
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="table">Table</param>
        /// <param name="isOn">是否开启</param>
        /// <returns>n</returns>
        public static void SetIdentityInsert<TEntity>(this Table<TEntity> table, bool isOn) where TEntity : class
        {
            Type entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            var tableName = "[" + metaTable.TableName + "]";
            string commandText = "SET   IDENTITY_INSERT " + tableName + " " + (isOn ? "ON" : "OFF");
            DbCommand cmd = (table.Context as SqlDataContext).SqlCommand;
            cmd.CommandText = commandText;
            cmd.Transaction = table.Context.Transaction;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// SqlBulkCopy批量Copy数据
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="table">Table</param>
        /// <param name="entities">条件</param>
        /// <param name="evaluator">更新字段</param>
        /// <returns>n</returns>
        public static bool BulkCopy<TEntity>(this Table<TEntity> table, List<TEntity> entitys) where TEntity : class
        {
            SqlBulkCopy sqlBulkCopy = (table.Context as SqlDataContext).SqlBulkCopy;
            Type entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            ReadOnlyCollection<MetaDataMember> dataMembers = metaTable.RowType.DataMembers;
            DataTable bulkCopyTable = new DataTable();
            foreach (MetaDataMember mm in dataMembers)
            {
                bulkCopyTable.Columns.Add(mm.Name, mm.Type);
            }

            foreach (var entity in entitys)
            {
                DataRow row = bulkCopyTable.NewRow();
                foreach (DataColumn column in bulkCopyTable.Columns)
                {
                    row[column.ColumnName] = entityType.GetProperty(column.ColumnName).GetValue(entity, null);
                }
                bulkCopyTable.Rows.Add(row);
            }
            var tableName = "[" + metaTable.TableName + "]";
            sqlBulkCopy.BatchSize = 20000;
            sqlBulkCopy.BulkCopyTimeout = 0;
            sqlBulkCopy.NotifyAfter = 10000;
            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.WriteToServer(bulkCopyTable);
            return true;
        }

        /// <summary>
        /// SqlBulkCopy批量Copy数据
        /// </summary>
        /// <param name="table">数据</param>
        /// <param name="tableName">表名称</param>
        /// <param name="sqlDataContext">SqlDataContext</param>
        /// <returns>是否成功</returns>
        internal static bool BulkCopy(this DataTable table, string tableName, SqlDataContext sqlDataContext)
        {
            SqlBulkCopy sqlBulkCopy = sqlDataContext.SqlBulkCopy;
            sqlBulkCopy.BatchSize = 20000;
            sqlBulkCopy.BulkCopyTimeout = 0;
            sqlBulkCopy.NotifyAfter = 10000;
            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.WriteToServer(table);
            return true;
        }

        private static DbCommand GetUpdateCommand<TEntity>(this Table<TEntity> table, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
        {
            DbCommand command = (table.Context as SqlDataContext).SqlCommand;
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();

            //查询条件表达式转换成SQL的条件语句
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(filter.Body, command);

            string commandWhere = command.CommandText;
            var set = new StringBuilder();
            var entityType = typeof(TEntity);
            var metaTable = table.Context.Mapping.GetTable(entityType);
            var tableName = "[" + metaTable.TableName + "]";
            evaluator.Visit<MemberInitExpression>(delegate (MemberInitExpression expression)
            {
                set.Append(GetDbSetStatement<TEntity>(expression, table, command));
                return expression;
            });
            command.CommandText = "UPDATE " + tableName + " " + set + " WHERE " + commandWhere;
            return command;
        }
    }
}