using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SalesPersonService
    {
        /// <summary>
        /// 销售员登录
        /// </summary>
        /// <param name="salesPerson"></param>
        /// <returns>登录成功返回销售员对象，登录失败返回null</returns>
        public SalesPerson SalesPersonLogin(SalesPerson salesPerson)
        {
            string sql = "SELECT SPName FROM SalesPerson Where SalesPersonId = @SalesPersonId AND LoginPwd = @LoginPwd";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@SalesPersonId", salesPerson.SalesPersonId),
                new SqlParameter("@LoginPwd", salesPerson.LoginPwd)
            };
            SqlDataReader reader = SqlHelper.GetReader(sql, param);
            if (reader.Read())
            {
                salesPerson.SPName = reader["SPName"].ToString();
            }
            else
            {
                salesPerson = null;
            }
            reader.Close();
            return salesPerson;
        }
    }
}
