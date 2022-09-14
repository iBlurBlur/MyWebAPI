

using Mapster;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.Entities;
using MyWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[EnableCors("C")]
    public class ProductController : ControllerBase
    {
        private readonly DekDueShopContext dekDueShopContext;

        public ProductController(DekDueShopContext dekDueShopContext)
        {
            this.dekDueShopContext = dekDueShopContext;
        }

        //localhost/product
        [HttpGet]
        //[DisableCors]
        public async Task<IEnumerable<ProductResponseDTO>>  GetProducts()
        {
            // select * From Product P
            // left join ProductCategory PC
            // on P.ProductCategoryID = PC.ProductCategory
            return await dekDueShopContext.Products
                        .Include(product => product.ProductCategory)
                        .ProjectToType<ProductResponseDTO>()
                        .ToListAsync();
        }

        //localhost/product/1234 => validation
        //localhost/product
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductByID(int id)
        {
            // select * from product where id = id
            Product? result = await dekDueShopContext.Products.FindAsync(id);
            if(result == null)
            {
                return NotFound();
            }

            return result.Adapt<ProductResponseDTO>();
        }

        //localhost/product/search?name=123
        [HttpGet("search")]
        public ActionResult<string> SearchProduct([FromQuery]string name)
        {
            return $"name {name}";
        }

        //localhost/product
        [HttpPost]
        public ActionResult<string> AddProduct([FromForm] CreateProductDTO productDTO) //{ form data }
        {
            var newProduct = new Product();

            productDTO.Adapt(newProduct);

            return newProduct.Name;
        }

        //localhost/product
        [HttpPut]
        public ActionResult<string> EditProduct(EditProductDTO productDTO)
        {
            return "put";
        }

        //localhost/product
        [HttpDelete]
        public ActionResult<string> DeleteProduct()
        {
            return "delete";
        }
    }
}
