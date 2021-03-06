using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 登录日志类
    /// </summary>
    [Serializable]
    public class LoginLog
    {
        /// <summary>
        /// 日志编号
        /// </summary>
        public int LogId { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public int LoginId { get; set; }
        /// <summary>
        /// 销售人员姓名
        /// </summary>
        public string SPName { get; set; }
        /// <summary>
        /// 登录主机名
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime ExitTime { get; set; }
    }
}
