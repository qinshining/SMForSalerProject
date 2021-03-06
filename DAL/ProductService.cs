using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ProductService
    {
        /// <summary>
        /// 根据商品编号查询商品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>商品信息对象，未能查询成功则返回null</returns>
        public Product GetProductInfo(string productId)
        {
            string sql = "SELECT ProductId,ProductName,UnitPrice,Unit,Discount FROM Products WHERE ProductId = @ProductId";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ProductId", productId)
            };
            SqlDataReader reader = SqlHelper.GetReader(sql, param);
            Product objProduct = null;
            if (reader.Read())
            {
                objProduct = new Product()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductFullName = reader["ProductName"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                    Unit = reader["Unit"].ToString(),
                    Discount = Convert.ToDouble(reader["Discount"])
                };
            }
            reader.Close();
            return objProduct;
        }
        /// <summary>
        /// 结算，更新销售主表、明细、会员积分
        /// </summary>
        /// <param name="objSalesList"></param>
        /// <param name="objMember"></param>
        /// <returns></returns>
        public bool SaveSaleInfo(SalesList objSalesList, Member objMember)
        {
            List<string> sqlList = new List<string>();
            StringBuilder sqlBuilder = new StringBuilder();
            //添加主表信息
            sqlBuilder.AppendFormat("INSERT INTO SalesList (SeriaNum,TotalMoney,RealReceive,ReturnMoney,SalesPersonId) VALUES ('{0}',{1},{2},{3},{4})", objSalesList.SeriaNum, objSalesList.TotalMoney, objSalesList.RealReceive, objSalesList.ReturnMoney, objSalesList.SalesPersonId);
            sqlList.Add(sqlBuilder.ToString());
            //添加明细信息
            foreach (SalesListDetail detailItem in objSalesList.SalesListDetail)
            {
                sqlBuilder.Clear();
                sqlBuilder.AppendFormat("INSERT INTO SalesListDetail (SerialNum,ProductId,ProductName,UnitPrice,Discount,Quantity,SubTotalMoney) VALUES('{0}', '{1}', '{2}',{3},{4},{5},{6})", detailItem.SerialNum, detailItem.ProductId, detailItem.ProductFullName, detailItem.UnitPrice, detailItem.Discount, detailItem.Quantity, detailItem.SubTotalMoney);
                sqlList.Add(sqlBuilder.ToString());
                //减少库存
                sqlBuilder.Clear();
                sqlBuilder.AppendFormat("UPDATE ProductInventory SET TotalCount = TotalCount - {0} WHERE ProductId = '{1}'", detailItem.Quantity, detailItem.ProductId);
                sqlList.Add(sqlBuilder.ToString());
            }
            //有会员信息的话，更新会员积分
            if (objMember != null)
            {
                sqlBuilder.Clear();
                sqlBuilder.AppendFormat("UPDATE SMMembers SET Points = Points + {0} WHERE MemberId = {1}", objMember.Points, objMember.MemberId);
                sqlList.Add(sqlBuilder.ToString());
            }
            return SqlHelper.UpdateByTran(sqlList);
        }
    }
}
