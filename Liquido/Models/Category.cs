using System.ComponentModel.DataAnnotations;

namespace Liquido.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100 , MinimumLength =2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }   
        public bool IsActive { get; set; } = true;  

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
