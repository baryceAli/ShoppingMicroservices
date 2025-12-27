using ShoppingMicroservices.FrontEnd.Web.Models.Dto;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int CartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);

    }
}