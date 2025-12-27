using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}