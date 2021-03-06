using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DALCommon
    {
        /// <summary>
        /// 获取sqlserver数据库服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerTime()
        {
            string sql = "SELECT GETDATE()";
            return Convert.ToDateTime(SqlHelper.GetSingleResult(sql));
        }
    }
}
