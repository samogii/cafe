namespace Cafe.Models
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Price { get; set; }
        public bool? IsAvalible { get; set; }
    }
}
