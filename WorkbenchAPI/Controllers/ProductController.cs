using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;
using WorkbenchAPI.Models;
using WorkbenchAPI.Services;

namespace WorkbenchAPI.Controllers
{
    [Route("Shop/shop/{shopId}/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public ActionResult Create([FromRoute] int shopId, [FromBody] CreateProductDto dto )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newProductId = _productService.Create(shopId, dto);
            return Created($"[controller]/shop/{shopId}/product/{newProductId}",null);


        }
     
        [HttpGet]
        public ActionResult<List<ProductDto>> Get([FromRoute] int shopId)
        {
            var productdtos = _productService.GetAll(shopId);
            return Ok(productdtos);
        }

        [HttpGet("{productId}")]
        public ActionResult<ProductDto> GetById([FromRoute] int shopId, [FromRoute] int productId)
        {
            var product = _productService.GetById(shopId, productId);
            return Ok(product);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int shopId)
        {
            _productService.Delete(shopId);
            return NoContent();
        }


        [HttpDelete("{productId}")]
        public ActionResult DeleteById([FromRoute] int shopId, [FromRoute] int productId)
        {
            _productService.DeleteById(shopId, productId);
            return NoContent();
        }



    }
}
