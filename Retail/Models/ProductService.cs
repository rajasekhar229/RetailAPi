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
            Products GetProductNameandPrice(Products product,bool productname);
            Products GetByIdforupdate(int Id);
        
        }
        public class Productservice : IProductService
        {
            public Products GetById(int Id)
            {
                 if (!Directory.Exists(@"C:\litedb"))
                 {
                     Directory.CreateDirectory(@"C:\litedb");
                 }
                 
                 using (var db = new LiteDatabase(@"C:\litedb\MyData.db"))
                 {
                     var Existingproducts = db.GetCollection<Products>("Products")
                                               .Find(x => x.Id == Id).FirstOrDefault();
                     if (Existingproducts == null)
                     { 
                         // Get a collection (or create, if doesn't exist)
                         var col = db.GetCollection<ProductsLiteDb>("Products");
                         Dictionary<string, string> hash = new Dictionary<string, string>
                         {
                             { "value", "" },
                             { "currency_code", "USD" }
                         };
                         var product = new Products()
                         {
                             Id = Id,
                             CurrentPrice = hash
                         };
                         var productlitedb = new ProductsLiteDb() 
                         {
                             Id = Id,
                             CurrentPrice = hash
                         };
                        var updatedproductfromredsky = GetProductNameandPrice(product); 
                         // if redsky has no item dont add it litedb
                         if (!(updatedproductfromredsky.ProductName==null && updatedproductfromredsky.CurrentPrice["value"]==""))
                         {
                            productlitedb.CurrentPrice["value"]=updatedproductfromredsky.CurrentPrice["value"];
                            col.Insert(productlitedb);
                         }
                     }
                 
                       // Get Product collection
                       var products = db.GetCollection<Products>("Products")
                                     .Find(x => x.Id == Id).FirstOrDefault(); 
                       return products;
                 }
            }
            public Products GetByIdforupdate(int Id)
            {
                try
                { 
                    using (var db = new LiteDatabase(@"C:\litedb\MyData.db"))
                    {
                    // Get Product collection
                    var products = db.GetCollection<Products>("Products")
                                      .Find(x => x.Id == Id).FirstOrDefault();
                    return products;
                    }
                }
                catch(Exception e)
                {
                   return  null;
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
                }
            }

            public Products GetProductNameandPrice(Products product,bool productname=false)
            {
                try { 
                       string path = "http://redsky.target.com/v2/pdp/tcin/" + product.Id + "?excludes=taxonomy,promotion,bulk_ship,rating_and_review_reviews,rating_and_review_statistics,question_answer_statistics";
                       var client = new WebClient();
                       var response = client.DownloadString(path);
                       JObject jObject = JObject.Parse(response);
                       
                       if (jObject != null && !productname)
                       {
                           product.ProductName = jObject["product"]["item"]["product_description"]["title"].ToString();
                           product.CurrentPrice["value"] = jObject["product"]["price"]["listPrice"]["price"].ToString();
                       }
                       if (jObject != null && productname)
                       {
                           product.ProductName = jObject["product"]["item"]["product_description"]["title"].ToString(); 
                       }

                return product;
                }
                catch (WebException wex)
                {
                    // returning products fro 404 erro if product is not in redsky 
                    // and not filling into litedb in get by id
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        return product;
                    }
                    return product;
                }
            
            }
        }
}