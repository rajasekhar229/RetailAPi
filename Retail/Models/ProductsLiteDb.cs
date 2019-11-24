using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retail.Models
{
    public class ProductsLiteDb
    { 
            public int Id { get; set; } 
            public Dictionary<string, string> CurrentPrice { get; set; }  
    }
}