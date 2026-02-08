using ApiForAng.Models;

namespace ApiForAng.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public string ?size { get; set; }

        public string ?ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
