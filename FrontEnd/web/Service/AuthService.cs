using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            this._baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.AuthAPIBase}/api/Auth/AssignRole",
                Data = registerationRequestDto
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.AuthAPIBase}/api/Auth/login",
                Data = loginRequestDto
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Url = $"{SD.AuthAPIBase}/api/Auth/register",
                Data = registerationRequestDto
            });
        }
    }

}