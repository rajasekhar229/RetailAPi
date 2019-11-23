using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Retail.Models; 

namespace Retail.Controllers
{
    public class ProductsController : ApiController
    {
        // GET: Products
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("api/products")]
        // GET: api/Product
        public HttpResponseMessage AddDatatoliteDb()
        {
            var item = _productService.adddatatolitedb();

            if (item.Count ==0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        [Route("api/products/{id}")]
        // GET: api/Product/5
        public HttpResponseMessage Get(int id)
        {
            var item = _productService.GetById(id);

            if (item == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, id);
            }

            string ProductName = _productService.GetProductName();
            item.ProductName = ProductName; 

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPut]
        [Route("api/products/{id}")]
        // PUT: api/Product/5
        public HttpResponseMessage Put( [FromBody]Products products)
        {
            int id = products.Id;
            var item = _productService.GetById(id);

            if (item == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, id);
            }
            else
            {
               var updateditem = _productService.Update(products);
                return Request.CreateResponse(HttpStatusCode.OK, updateditem);
            }
        }
    }
}
