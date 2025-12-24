using ShoppingMicroservices.FrontEnd.Web.Models.Dto;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string code);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(AddCouponDto addCouponDto);
        Task<ResponseDto?> UpdateCouponAsync(int id, AddCouponDto addCouponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);

    }
}