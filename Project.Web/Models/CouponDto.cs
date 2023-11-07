using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        [Required]
        [Range(10, int.MaxValue, ErrorMessage = "Value must be positive")]
        public double DiscountAmount { get; set; }
        [Required]
        [Range(10, int.MaxValue, ErrorMessage = "Value must be positive")]
        public int MinAmount { get; set; }
    }
}
