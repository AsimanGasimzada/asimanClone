using System.ComponentModel.DataAnnotations.Schema;

namespace JaleIdentity.Dtos.ProductDtos;

public class ProductPostDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public IFormFile Image { get; set; } = null!;
    public int CategoryId { get; set; }
}
