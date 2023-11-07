using Project.Web.Utility;
using System.ComponentModel.DataAnnotations;


namespace Project.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be positive")]
        public double Price { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }

        [Range(1,20)]
        public int Count { get; set; } = 1;
        [Required]
        [MaxFileSize(1)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile? Image { get; set; }
    }
}
