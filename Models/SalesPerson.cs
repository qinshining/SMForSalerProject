using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class SalesPerson
    {
        public int SalesPersonId { get; set; }
        public string SPName { get; set; }
        public string LoginPwd { get; set; }
        //扩展属性
        public int LogId { get; set; }//登录日志ID
    }
}
