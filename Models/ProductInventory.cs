using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 商品库存类
    /// </summary>
    public class ProductInventory
    {
        public string ProductId { get; set; }
        public int TotalCount { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
