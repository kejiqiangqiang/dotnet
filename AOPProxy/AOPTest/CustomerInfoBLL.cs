using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections;
using BusinessProxy;
using Business.Dao;


namespace AOPTest.Business
{
    /// <summary>
    /// 会员信息业务层
    /// </summary>
    public class CustomerInfoBLL : BaseBusiness
    {
        public CustomerInfoDao CustomerInfoDao { get; set; }

        /// <summary>
        /// 判断会员名是否存在
        /// </summary>
        [BusinessMethod]
        public string ValidCustomerName(string fieldValue)
        {
            if (CustomerInfoDao.Entity.Where(x => x.CustomerName == fieldValue).Count() > 0)
            {
                //该会员名已存在
                return "[\"CustomerName\",false,\"会员名已存在\"]";
            }
            return "[\"CustomerName\",true,\"\"]";
        }

        /// <summary>
        /// 判断邮箱是否存在
        /// </summary>
        [BusinessMethod]
        public string ValidEmail(string fieldValue)
        {
            if (CustomerInfoDao.Entity.Where(x => x.Email == fieldValue).Count() > 0)
            {
                return "[\"Email\",false,\"邮箱已存在\"]";
            }
            return "[\"Email\",true,\"\"]";
        }

        /// <summary>
        /// 判断旧密码是否正确
        /// </summary>
        [BusinessMethod]
        public string ValidPassword(string fieldValue)
        {
            return "[\"oldPass\",true,\"\"]";
        }

        /// <summary>
        /// 会员资料修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [BusinessMethod]
        public bool Update(CustomerInfo model)
        {
            CustomerInfoDao.UpdateWhere(p => p.CustomerID == model.CustomerID, p => new CustomerInfo
            {
                CompAddress = model.CompAddress,
                CompName = model.CompName,
                CompUrl = model.CompUrl,
                CustomerName = model.CustomerName,
                Email = model.Email,
                Linkman = model.Linkman,
                Phone = model.Phone,
                UserNo = model.UserNo,
                Remark = model.Remark
            });
            return true;
        }

        /// <summary>
        /// 会员删除
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [BusinessMethod]
        public bool Delete(int CustomerID)
        {
            CustomerInfoDao.Delete(x => x.CustomerID == CustomerID);
            return true;
        }


        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [BusinessMethod]
        public CustomerInfo GetModel(int CustomerID)
        {
            CustomerInfo model = CustomerInfoDao.GetModel(CustomerID);
            return model;
        }

        /// <summary>
        /// 获取会员信息列表
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [BusinessMethod]
        public List<CustomerInfo> GetList(int CustomerID)
        {
            return CustomerInfoDao.Entity.ToList();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="total">总行数</param>
        /// <param name="sortName">排序列</param>
        /// <param name="sortOrder">排序方向</param>
        /// <param name="search">关键字</param>
        /// <returns>数据</returns>
        [BusinessMethod]
        public IList GetCustomerInfo(int pageSize, int pageIndex, out int total, string sortName, string sortOrder, string search)
        {
            IQueryable<CustomerInfo> models = CustomerInfoDao.Entity;
            var customer = CustomerInfoDao.Entity;
            var query = (from c in customer
                         select new
                         {
                             c.CustomerID,
                             c.CustomerName,
                             c.Email,
                             c.CompName,
                             c.CompAddress,
                             c.CompUrl,
                             c.Linkman,
                             c.Phone,
                             c.State,
                             c.UserNo,
                         });

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                query = query.Where(x => x.CustomerName.Contains(search));
            }
            query = query.OrderBy(model => model.CustomerID);
            total = int.Parse(query.LongCount().ToString());
            IList list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return list;
        }

    }
}
