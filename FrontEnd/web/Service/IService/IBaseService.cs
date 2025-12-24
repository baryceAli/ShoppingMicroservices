using ShoppingMicroservices.FrontEnd.Web.Models.Dto;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}