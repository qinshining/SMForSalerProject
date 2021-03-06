using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class LoginLogService
    {
        /// <summary>
        /// 写入登录日志，返回日志Id
        /// </summary>
        /// <param name="loginLog"></param>
        /// <returns></returns>
        public int WriteLoingLog(LoginLog loginLog)
        {
            string sql = "INSERT INTO LoginLogs (LoginId,SPName,ServerName) VALUES (@LoginId,@SPName,@ServerName);SELECT @@IDENTITY";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@LoginId", loginLog.LoginId),
                 new SqlParameter("@SPName", loginLog.SPName),
                  new SqlParameter("@ServerName", loginLog.ServerName)
            };
            return Convert.ToInt32(SqlHelper.GetSingleResult(sql, param));
        }
        /// <summary>
        /// 更新日志，记录退出系统时间
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="exitTime"></param>
        /// <returns></returns>
        public int WriteExitLog(int logId, DateTime exitTime)
        {
            string sql = "UPDATE LoginLogs SET ExitTime = @ExitTime WHERE LogId = @LogId";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ExitTime", exitTime),
                new SqlParameter("@LogId", logId)
            };
            return SqlHelper.ExecuteUpdate(sql, param);
        }
    }
}
