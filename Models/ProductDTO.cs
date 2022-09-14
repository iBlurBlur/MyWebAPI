using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class BaseProductDTO
{
    [Required, MaxLength(100)]
    [MinLength(3, ErrorMessage = "Product name must be more than 3 characters.")]
    public string Name { get; set; } = null!;
    [Required, MaxLength(25)]
    public string ProductNumber { get; set; } = null!;
    [MaxLength(15)]
    public string? Color { get; set; }
    [Required(ErrorMessage = "The value Price is Required")]
    [Range(0, 100_000)]
    public decimal Price { get; set; }
    [MaxLength(5)]
    public string? Size { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
    public decimal? Weight { get; set; }
    [MaxLength(50)]
    public string? ThumbnailPhotoFileName { get; set; }
}

public class ProductResponseDTO : BaseProductDTO
{
    [Required]
    public int ProductId { get; set; }
    public byte[]? ThumbNailPhoto { get; set; }
    public ProductCategoryDTO? ProductCategory { get; set; }
}

public class CreateProductDTO : BaseProductDTO
{
    [Required(ErrorMessage = "Please select a file.")]
    [DataType(DataType.Upload)]
    public IFormFile? UploadFile { get; set; }
    [Required]
    public int ProductCategoryId { get; set; }
}

public class EditProductDTO : CreateProductDTO
{
    [Required]
    public int ProductId { get; set; }
}
