using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 销售主类
    /// </summary>
    public class SalesList
    {
        public string SeriaNum { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal RealReceive { get; set; }
        public decimal ReturnMoney { get; set; }
        public int SalesPersonId { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SalesListDetail> SalesListDetail { get; set; }
        public SalesList()
        {
            this.SalesListDetail = new List<SalesListDetail>();
        }
    }
}
