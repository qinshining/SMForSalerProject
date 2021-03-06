using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 商品信息类
    /// </summary>
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductFullName { get; set; }//ProductName和Control控件字段同名，在dgv中name为ProductFullName（数据源还是ProductName）
        public decimal UnitPrice { get; set; }//单价
        public string Unit { get; set; }//单位
        public double Discount { get; set; }//todo 可能出现不准的问题
    }
}
