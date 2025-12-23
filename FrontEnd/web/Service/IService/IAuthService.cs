using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registerationRequestDto);
    }
}