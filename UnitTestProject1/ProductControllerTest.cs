using Microsoft.VisualStudio.TestTools.UnitTesting; 
using Newtonsoft.Json.Linq;
using NSubstitute;
using Retail.Controllers;
using Retail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UnitTestProject1
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void GetAllProducts_ReturnProductsById()
        {
            // Arrange
            var productservice =Substitute.For<IProductService>();
            int _id = 13860428;
            var productlist = Productslist().Where(a => a.Id == _id).FirstOrDefault();
            productservice.GetById(_id).Returns(productlist);
            productservice.GetProductNameandPrice(productlist,true).Returns(productlist);
            var productcontroller = new ProductsController(productservice)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var productsfromcontroller = productcontroller.Get(_id).Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(productsfromcontroller);

            var products = productcontroller.Get(_id);
            Assert.IsTrue(products.TryGetContentValue<Products>(out Products product));

            // Assert
            Assert.AreEqual(productlist.ProductName, product.ProductName);
            Assert.AreEqual(_id, jObject["Id"]);
        }

        [TestMethod]
        public void GetAllProducts_PutProductsPrice()
        {
            // Arrange
            var productservice = Substitute.For<IProductService>();
            int _id = 13860429;
            var productlist = Productslist().Where(a => a.Id == _id).FirstOrDefault();
            productservice.GetById(_id).Returns(productlist);
            ProductsLiteDb litedbproducts = new ProductsLiteDb { Id = productlist.Id, CurrentPrice = productlist.CurrentPrice };
            productservice.GetByIdforupdate(_id).Returns(litedbproducts);
            //productservice.GetProductNameandPrice(productlist, true).Returns(productlist);
            var productcontroller = new ProductsController(productservice)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            productlist.CurrentPrice["value"] = "50";
            productservice.Update(litedbproducts).Returns(litedbproducts);

            // Act
            var productswew = productcontroller.Put(productlist).StatusCode; 

            // Assert 
            Assert.AreEqual(HttpStatusCode.OK, productswew);
        }


        private List<Products> Productslist()
        {
            List<Products> prd = new List<Products>()
            {
                new Products(){ Id=13860428,ProductName="The Big Lebowski (Blu-ray) ",
                                CurrentPrice =new Dictionary<string, string>
                                { { "value", "13.49" },{ "currency_code", "USD" } } },
                new Products(){ Id=13860429,ProductName="",
                                CurrentPrice =new Dictionary<string, string>
                                { { "value", "14.49" },{ "currency_code", "USD" } } },
                new Products(){ Id=13860430,ProductName="LCD TV",
                                CurrentPrice =new Dictionary<string, string>
                                { { "value", "15.49" },{ "currency_code", "USD" } } }

            };
            return prd;
        }
    }
}
