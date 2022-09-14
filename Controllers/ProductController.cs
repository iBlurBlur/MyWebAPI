

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
        public IActionResult AddProduct([FromForm] CreateProductDTO productDTO) //{ form data }
        {
            Product product = productDTO.Adapt<Product>();

            IFormFile? file = productDTO.UploadFile;
            product.ThumbNailPhoto = ConvertFileToByte(file);

            dekDueShopContext.Products.Add(product);
            dekDueShopContext.SaveChanges();

            var newProduct = product.Adapt<ProductResponseDTO>();
            return Created($"Product/{product.ProductId}", newProduct);
        }

        //localhost/product/{id}
        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, [FromForm] EditProductDTO productDTO)
        {
            if(id != productDTO.ProductId)
            {
                return BadRequest();
            }

            Product? result = dekDueShopContext.Products.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            productDTO.Adapt(result);

            IFormFile? file = productDTO.UploadFile;
            if(file != null)
            {
                result.ThumbNailPhoto = ConvertFileToByte(file);
            }

            dekDueShopContext.SaveChanges();
            return NoContent();
        }

        //localhost/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? result = await dekDueShopContext.Products.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            dekDueShopContext.Products.Remove(result);
            await dekDueShopContext.SaveChangesAsync();

            return NoContent(); 
        }

        public static byte[]? ConvertFileToByte(IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return fileBytes;
            }
            return null;
        }
    }
}
