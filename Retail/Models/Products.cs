using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retail.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public Dictionary<string, string> CurrentPrice { get; set; }
        //public Dictionary<double,string> CurrencyCode { get; set; }
    }
}