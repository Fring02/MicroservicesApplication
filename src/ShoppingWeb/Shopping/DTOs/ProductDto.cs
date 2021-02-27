using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shopping.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required, StringLength(80)]
        public string Category { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile File { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
