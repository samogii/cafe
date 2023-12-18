using System.ComponentModel.DataAnnotations;

namespace Cafe.Models
{
    public class Product
    {
        public Product(string name, string price, string description, string image)
        {
            Name = name;
            Price = price;
            Description = description;
            IsAvalible = true;
            Image = image;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Price { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsAvalible { get; set; }
    }
}
