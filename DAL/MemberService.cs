using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MemberService
    {
        /// <summary>
        /// 根据会员编号获取会员对象
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public Member GetMemberById(string memberId)
        {
            string sql = "SELECT MemberName,Points,PhoneNumber,MemberAddress,OpenTime,MemberStatus FROM SMMembers WHERE MemberId = @MemberId AND MemberStatus = 1";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@MemberId", memberId)
            };
            SqlDataReader objReader = SqlHelper.GetReader(sql, param);
            Member objMember = null;
            if (objReader.Read())
            {
                objMember = new Member()
                {
                    MemberId = Convert.ToInt32(memberId),
                    MemberName = objReader["MemberName"].ToString(),
                    Points = Convert.ToInt32(objReader["Points"]),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    MemberAddress = objReader["MemberAddress"].ToString(),
                    OpenTime = Convert.ToDateTime(objReader["OpenTime"]),
                    MemberStatus = (MemberStatus)Convert.ToInt32(objReader["MemberStatus"]),
                };
            }
            objReader.Close();
            return objMember;
        }
        /// <summary>
        /// 根据卡号判断会员是否存在
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool IsMemberExists(string memberId)
        {
            string sql = "SELECT COUNT(1) FROM SMMembers WHERE MemberId = @MemberId AND MemberStatus = 1";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@MemberId", memberId)
            };
            int result = Convert.ToInt32(SqlHelper.GetSingleResult(sql, param));
            return result == 1;
        }
    }
}
