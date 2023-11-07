using Project.Services.ShoppingCartAPI.Models.Dto;

namespace Project.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponSerivce
    {
        Task<CouponDto> GetCoupon(string couponCode);

    }
}
