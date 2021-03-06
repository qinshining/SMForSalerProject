using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 超市会员类
    /// </summary>
    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int Points { get; set; }
        public string PhoneNumber { get; set; }
        public string MemberAddress { get; set; }
        public DateTime OpenTime { get; set; }
        public MemberStatus MemberStatus { get; set; }//会员卡状态（1：正常使用；0：冻结；-1：注销）
    }
    public enum MemberStatus
    {
        Cancellation = -1,
        Frozen = 0,
        Normal = 1
    }
}
