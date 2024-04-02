namespace JaleIdentity.Dtos.ProductDtos;

public class ProductPutDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public int CategoryId { get; set; }
}
