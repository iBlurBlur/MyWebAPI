namespace MyWebAPI.Models;

public class ProductCategoryDTO
{
    public int ProductCategoryId { get; set; }
    public string Name { get; set; } = null!;
}

public class ParentProductCategoryDTO : ProductCategoryDTO
{
    public string ParentName { get; set; } = null!;
}
