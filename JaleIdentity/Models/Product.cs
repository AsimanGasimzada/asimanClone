namespace JaleIdentity.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = null!;
  
    public Category Category { get; set; }=null!;
    public int CategoryId { get; set; }


}