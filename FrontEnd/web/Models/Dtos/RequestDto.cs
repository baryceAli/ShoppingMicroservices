using static ShoppingMicroservices.FrontEnd.Web.Utility.SD;

namespace ShoppingMicroservices.FrontEnd.Web.Models.Dtos
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object? Data { get; set; }
        public string AccessToken { get; set; }
    }
}