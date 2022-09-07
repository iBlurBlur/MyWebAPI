

using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        //localhost/product
        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            return "tanakorn";
        }

        //localhost/product/1234 => validation
        //localhost/product
        [HttpGet("{id}")]
        public ActionResult<string> GetProductByID(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            return $"id {id}";
        }

        //localhost/product/search?name=123
        [HttpGet("search")]
        public ActionResult<string> SearchProduct([FromQuery]string name)
        {
            return $"name {name}";
        }

        //localhost/product
        [HttpPost]
        public ActionResult<string> AddProduct(ProductDTO productDTO) //{ json }
        {
            var newProduct = new Product();

            productDTO.Adapt(newProduct);

            return newProduct.Name;
        }

        //localhost/product
        [HttpPut]
        public ActionResult<string> EditProduct()
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

public class ProductDTO
{
    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public decimal Price { get; set; }

    public string? Size { get; set; }

    public decimal? Weight { get; set; }

    public string? ThumbnailPhotoFileName { get; set; }
}

public class Product
{
    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public decimal Price { get; set; }

    public string? Size { get; set; }

    public decimal? Weight { get; set; }

    public string? ThumbnailPhotoFileName { get; set; }

    public bool Expired { get; set; }
}
