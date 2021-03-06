using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 销售明细类
    /// </summary>
    public class SalesListDetail
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public string ProductId { get; set; }
        public string ProductFullName { get; set; }
        public decimal UnitPrice { get; set; }
        public double Discount { get; set; }//todo 可能出现不准的问题
        public int Quantity { get; set; }
        public decimal SubTotalMoney { get; set; }
        //扩展属性
        public int Num { get; set; }//序号
    }
}
