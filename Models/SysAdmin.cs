using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class SysAdmin
    {
        public int LoginId { get; set; }
        public string LoginPwd { get; set; }
        public string AdminName { get; set; }
        public bool AdminStatus { get; set; }//是否停用
        public int RoleId { get; set; }
    }
}
