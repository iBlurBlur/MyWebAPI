

using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        //localhost/product
        [HttpGet]
        public ActionResult<ProductDTO> GetProducts()
        {
            var product = new Product
            {
                Name = "A1",
            };

            ProductDTO productDTO = product.Adapt<ProductDTO>();

            return productDTO;
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
        public ActionResult<string> AddProduct([FromForm] CreateProductDTO productDTO) //{ form data }
        {
            var newProduct = new Product();

            productDTO.Adapt(newProduct);

            return newProduct.Name;
        }

        //localhost/product
        [HttpPut]
        public ActionResult<string> EditProduct(ProductDTO productDTO)
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

public class ProductDTO : CreateProductDTO
{
    public int ProductId { get; set; }
}

public class CreateProductDTO
{
    [Required, MaxLength(100)]
    [MinLength(3, ErrorMessage = "Product name must be more than 3 characters.")]
    public string Name { get; set; } = null!;
    [Required, MaxLength(25)]
    public string ProductNumber { get; set; } = null!;
    [MaxLength(15)]
    public string? Color { get; set; }
    [Range(0, 100_000)]
    public decimal Price { get; set; }
    [MaxLength(5)]
    public string? Size { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
    public decimal? Weight { get; set; }
    public IFormFile? ThumbNailPhoto { get; set; }
    [MaxLength(50)]
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

    public byte[]? ThumbNailPhoto { get; set; }

    public bool Expired { get; set; }
}
