using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

using BusinessProxy;
using BusinessProxy.LinqToSQL.Extend.Core.Linq;//使用扩展方法必须添加其引用--不加则无法使用扩展方法，而此时提示不会明确(因为扩展方法为静态类的静态方法),因此扩展方法最好和调用对象在同一命名空间

namespace Business.Dao
{
    /// <summary>
    /// 会员信息表数据访问
    /// </summary>
    public partial class CustomerInfoDao : TemplateDao<CustomerInfo>
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="CustomerID">会员编号</param>
        public CustomerInfo GetModel(int CustomerID)
        {
            Table<CustomerInfo> models = this.Entity;
            var result = from model in models
                         where model.CustomerID == CustomerID
                         select model;
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 更新或增加实体
        /// </summary>
        /// <param name="model">实体对象</param>
        public void SaveOrUpdate(CustomerInfo model)
        {
            bool isExists = this.IsExists(model.CustomerID);
            if (isExists)
            {
                this.Update(model);
            }
            else
            {
                this.Add(model);
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="CustomerID">会员编号</param>
        public int Delete(int CustomerID)
        {
            Table<CustomerInfo> tables = this.Entity;
            int n = tables.Delete(model => model.CustomerID == CustomerID);
            return n;
        }

        /// <summary>
        /// 是否存在实体
        /// </summary>
        /// <param name="CustomerID">会员编号</param>
        public bool IsExists(int CustomerID)
        {
            Table<CustomerInfo> models = this.Entity;
            var result = from model in models
                         where model.CustomerID == CustomerID
                         select model;
            return result.Count() > 0;
        }

        /// <summary>
        /// Linq2SQL单表分页
        /// </summary>
        /// <param name="count">行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据</returns>
        public override IQueryable<CustomerInfo> LoadAllWithPage(out int count, int pageIndex, int pageSize)
        {
            Table<CustomerInfo> models = this.Entity;
            count = int.Parse(models.LongCount().ToString());
            IQueryable<CustomerInfo> query = models.OrderBy(model => model.CustomerID);
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }


    /// <summary>
    /// 会员信息表
    /// </summary>
    [Serializable]
    [Table(Name = "CustomerInfo")]
    public class CustomerInfo
    {

        /// <summary>
        /// 会员编号
        /// </summary>
        [Column(AutoSync = AutoSync.OnInsert, IsDbGenerated = true, DbType = "int NOT NULL IDENTITY", CanBeNull = false, IsPrimaryKey = true)]
        public int CustomerID { get; set; }

        /// <summary>
        /// 会员名
        /// </summary>
        [Column(DbType = "NVarChar(60) NOT NULL", CanBeNull = false)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(DbType = "NVarChar(60) NOT NULL", CanBeNull = false)]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(DbType = "NVarChar(200) NOT NULL", CanBeNull = false)]
        public string Password { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [Column(DbType = "NVarChar(60) NOT NULL", CanBeNull = false)]
        public string CompName { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        [Column(DbType = "NVarChar(200)", CanBeNull = true)]
        public string CompAddress { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Column(DbType = "NVarChar(60) NOT NULL", CanBeNull = false)]
        public string Linkman { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Column(DbType = "NVarChar(60)", CanBeNull = true)]
        public string Phone { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int State { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Column(DbType = "DateTime", CanBeNull = true)]
        public DateTime? RegTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Column(DbType = "DateTime", CanBeNull = true)]
        public DateTime? CheckTime { get; set; }

        /// <summary>
        /// 所属销售工号
        /// </summary>
        [Column(DbType = "NVarChar(60)", CanBeNull = true)]
        public string UserNo { get; set; }

        /// <summary>
        /// 邮箱激活码
        /// </summary>
        [Column(DbType = "VarChar(100)", CanBeNull = true)]
        public string ActivateCode { get; set; }

        /// <summary>
        /// 企业网址
        /// </summary>
        [Column(DbType = "VarChar(100)", CanBeNull = true)]
        public string CompUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(DbType = "VarChar(-1)", CanBeNull = true)]
        public string Remark { get; set; }

    }


}
