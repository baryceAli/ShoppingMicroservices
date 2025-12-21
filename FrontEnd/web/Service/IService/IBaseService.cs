using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}