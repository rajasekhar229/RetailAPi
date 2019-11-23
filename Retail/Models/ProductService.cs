using LiteDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;

namespace Retail.Models
{
     public interface IProductService
        {
            Products GetById(int Id);
            Products Update( Products product);
            List<Products> adddatatolitedb();
            string GetProductName();
        }
     public class Productservice : IProductService
        {
            public Products GetById(int Id)
            {

                using (var db = new LiteDatabase(@"C:\litedb\MyData.db"))
                {
                // Get products collection
                    var products = db.GetCollection<Products>("Products")
                                         .Find(x => x.Id == Id).FirstOrDefault(); 
                    return products;
                }

            }

            public Products Update(Products product)
            {
                using (var db = new LiteDatabase(@"C:\litedb\MyData.db"))
                {
                    // Get products collection
                    var products = db.GetCollection<Products>("Products")
                                     .Update(product);
                    return product;
                    //return GetById(product.Id);
                }
            }
            public List<Products> adddatatolitedb()
            {
                
                 if (!Directory.Exists(@"C:\litedb"))
                 {
                     Directory.CreateDirectory(@"C:\litedb");
                 }

                 using (var db = new LiteDatabase(@"C:\litedb\MyData.db"))
                 {
                     // Get a collection (or create, if doesn't exist)
                     var col = db.GetCollection<ProductForLiteDb>("Products");
                     Dictionary<string, string> hash = new Dictionary<string, string>(); 
                      hash.Add("value", "13.49");
                      hash.Add("currency_code","USD");
                     var Products = new ProductForLiteDb
                     {
                         Id = 13860428,
                         CurrentPrice = hash
                     };
                     
                     var Existingproducts = db.GetCollection<Products>("Products")
                                          .Find(x => x.Id == Products.Id).FirstOrDefault();
                     
                           if(Existingproducts==null)
                           {  
                               col.Insert(Products);
                           }
                           
                           // Get Product collection
                           var products = db.GetCollection<Products>("Products").FindAll().ToList();
                         return products;
                 } 
            }
            public  string GetProductName()
            {
                string path = "http://redsky.target.com/v2/pdp/tcin/13860428?excludes=taxonomy,price,promotion,bulk_ship,rating_and_review_reviews,rating_and_review_statistics,question_answer_statistics";
                string ProductName = "";
                var client = new WebClient(); 
                var response = client.DownloadString(path);
                JObject jObject = JObject.Parse(response); 

                if(jObject!=null)
                {
                   ProductName = jObject["product"]["item"]["product_description"]["title"].ToString();
                }
                     
                return ProductName;
            }
    }
   
}