using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace ConsoleApplication
{
    class Program
    {
        #region switch 测试
        //static void Main(string[] args)
        //{
        //    R("MSSQL");
            
        //}
        //public static string R(string s)
        //{
        //    string str = "";
        //    switch (s)
        //    {
        //        case "MSSQL":
        //            str += "MSSQL";
        //            return str;
        //        case "Oracle":
        //        case "MySQL":
        //            return "MySQL";
        //        case "Informix":
        //            return "Informix";
        //        case "Access":
        //            return "Access";
        //        default:
        //            return null;
                
        //    }
        //}
        #endregion

        #region 自定义比较器
        static void Main(string[] args)
        {
            //List<Acc_UserInfo> users = bll.MasterRedisConfig.Acc_UserInfo;
            //var user = users.Where(p => p.UserID == 1).FirstOrDefault();
            //var model = new Acc_UserInfo()
            //{
            //    Account = user.Account,
            //    BlocCode = user.BlocCode,
            //    UserID = user.UserID,
            //    DeptCode = user.DeptCode,
            //    DeviceToken = user.DeviceToken,
            //    EMail = user.EMail,
            //    FunctionRoleID = user.FunctionRoleID,
            //    HeadPortrait = user.HeadPortrait,
            //    IsCustomerServices = user.IsCustomerServices,
            //    IsPublic = user.IsPublic,
            //    IsStaOnline = user.IsStaOnline,
            //    IsUse = user.IsUse,
            //    OpenId = user.OpenId,
            //    Password = user.Password,
            //    Phone = user.Phone,
            //    RongCloudToken = user.RongCloudToken,
            //    Title = user.Title,
            //    UserName = user.UserName,
            //    UserType = user.UserType,
            //    WeChatNo = user.WeChatNo, CreateTime = user.CreateTime, ExpirationDate = user.ExpirationDate, NXTOpenId=user.NXTOpenId, Remark=user.Remark
            //};
            
            //users.Add(model);
            //var rs = users.OrderBy(p => p.UserID).Distinct(new Comparer<Acc_UserInfo>()).ToList();

            //var rs0 = users.Union(users, new Comparer<Acc_UserInfo>()).ToList();

            //var rs1 = users.Union(users).ToList(); //引用相同

            //var rs2 = users.Union(users).ToList();


        }
        #endregion

    }
    #region 自定义比较器--实现接口IEqualityComparer
    /// <summary>
    /// 比较器比较两个对象是否相等--对象中字段不能是对象（递归比较实现对象中对象比较）
    /// 2、首先遍历TSource，并调用此方法，当此函数返回值相等，则去调用Equals方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Comparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            bool isEqual = false;
            if (x != null && y != null)
            {
                isEqual = true;
                Type t = typeof(T);
                foreach (var item in t.GetProperties())
                {
                    Type propertyType = item.PropertyType;
                    Type getType = item.GetType();
                    Type getType_DeclaringType = item.GetType().DeclaringType;
                    Type declaringType = item.DeclaringType;

                    var obj1 = x.GetType().GetProperty(item.Name).GetValue(x, null);
                    var obj2 = y.GetType().GetProperty(item.Name).GetValue(y, null);

                    if (obj1 == null && obj2 != null || obj1 != null && obj2 == null || obj1 != null && obj2 != null 
                        && obj1.ToString() != obj2.ToString())//对象中字段不能是对象
                    {
                        isEqual = false;
                        break;
                    }
                }
            }
            if (isEqual)
            {
                
            }
            return isEqual;
        }

        /// <summary>
        /// 1、首先遍历TSource，并调用此方法，当此函数返回值相等，则去调用Equals方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            //引用类型：类对象相同即引用相同时，才会返回相同的HashCode
            //return obj.GetHashCode();
            //值类型：值相等则返回相同的HashCode

            //return obj.GetHashCode();
            return 0;

        }

    }

    #endregion
}
