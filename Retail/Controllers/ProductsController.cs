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
        [Route("products/{id}")]
        // GET: api/Product/5
        public HttpResponseMessage Get(int id)
        {
            var item = _productService.GetById(id);
            item = _productService.GetProductNameandPrice(item, true);
            if (item == null)
            {
                var message = string.Format("Product with id = {0} not found", id);
                return Request.CreateResponse(HttpStatusCode.NotFound, message);
            }
             return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpPut]
        [Route("products/{id}")]
        // PUT: api/Product/5
        public HttpResponseMessage Put( [FromBody]Products products)
        {
            int id = products.Id;
            // as we are adding our products through get by id 
            // we need new method to check if id exists in lite db 
            var item = _productService.GetByIdforupdate(id);

            if (item == null)
            {
                var message = string.Format("Product with id = {0} not found", id);
                return Request.CreateResponse(HttpStatusCode.NotFound, message);
            }
            else
            {

                // we are not saving name in lite db we are getting from redsky 
                products = _productService.GetProductNameandPrice(products, true);
                //use this and comment above method
                //if you want to save productname to database but only update price
                //item.CurrentPrice["value"] = products.CurrentPrice["value"];
                var updateditem = _productService.Update(products);
                return Request.CreateResponse(HttpStatusCode.OK, updateditem);
            }
        }
    }
}
