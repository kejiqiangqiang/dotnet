using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;

namespace BusinessProxy
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class SqlDataContext:DataContext
    {

        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public string TablePrefix { get; set; }
        /// <summary>
        /// 数据发生更改的表
        /// </summary>
        public List<string> DataTableChanged = new List<string>();

        /// <summary>
        /// 初始化数据库上下文
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="mapping">动态DynamicMappingSource</param>
        public SqlDataContext(string connectionString, DynamicMappingSource mapping)
            : base(connectionString, mapping)
        {
            mapping.GetMetaTableName = delegate(Type type)
            {
                string tableName;
                TableAttribute attr = type.GetCustomAttributes(typeof(TableAttribute), true).Single() as TableAttribute;
                tableName = attr.Name;
                if (!string.IsNullOrEmpty(this.TablePrefix))
                {
                    tableName = this.TablePrefix + tableName;
                }
                return tableName;
            };
        }

        SqlCommand _SqlCommand;
        SqlDataAdapter _SqlDataAdapter;
        SqlBulkCopy _SqlBulkCopy;
        private bool isDisposed = false;
        public bool IsDisposed
        {
            get { return isDisposed; }
        }
        public SqlCommand SqlCommand
        {
            get
            {
                if (this._SqlCommand == null || this.IsDisposed)
                {
                    this._SqlCommand = this.Connection.CreateCommand() as SqlCommand;
                }
                this._SqlCommand.Transaction = this.Transaction as SqlTransaction;
                return this._SqlCommand;
            }
        }

        public SqlDataAdapter SqlDataAdapter
        {
            get
            {
                if (this._SqlDataAdapter == null || this.IsDisposed)
                {
                    this._SqlDataAdapter = new SqlDataAdapter(this.SqlCommand);
                }
                return this._SqlDataAdapter;
            }
        }

        public SqlBulkCopy SqlBulkCopy
        {
            get
            {
                if (this._SqlBulkCopy == null || this.IsDisposed)
                {
                    SqlTransaction trans = this.Transaction as SqlTransaction;
                    if (trans!=null)
                    {
                        this._SqlBulkCopy = new SqlBulkCopy(this.Connection as SqlConnection, SqlBulkCopyOptions.CheckConstraints, trans);
                    }
                    else
                    {
                        this._SqlBulkCopy = new SqlBulkCopy(this.Connection as SqlConnection);
                    }
                }
                return this._SqlBulkCopy;
            }
        }

        
        protected override void Dispose(bool disposing)
        {
            if (this._SqlCommand!=null)
            {
                this._SqlCommand.Dispose();
                this._SqlCommand = null;
            }
            if (this._SqlDataAdapter != null)
            {
                this._SqlDataAdapter.Dispose();
                this._SqlDataAdapter = null;
            }
            if (this._SqlBulkCopy != null)
            {
                this._SqlBulkCopy.Close();
                this._SqlBulkCopy = null;
            }

            base.Dispose(disposing);

            this.isDisposed = true;
        }


    }
}