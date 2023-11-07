
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Value must be positive")]
        public int Count { get; set; }
    }
}
