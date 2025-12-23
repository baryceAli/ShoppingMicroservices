using System.Net;
using System.Text;
using System.Text.Json;
using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            this._httpClientFactory = httpClientFactory;
            this._tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("Shopping");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"bearer {token}");
                }
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonSerializer.Serialize(requestDto.Data), Encoding.UTF8, "application/json");

                }

                switch (requestDto.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;

                }

                HttpResponseMessage? apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDto() { Message = "Not found", isSuccess = false };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto() { Message = "Access denied", isSuccess = false };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto() { Message = "Unauthorized", isSuccess = false };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDto() { Message = "Internal server error", isSuccess = false };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var apiResponseDto = JsonSerializer.Deserialize<ResponseDto>(apiContent, options);
                        return apiResponseDto;
                }


            }
            catch (System.Exception ex)
            {
                ResponseDto responseDto = new()
                {
                    isSuccess = false,
                    Message = ex.Message
                };
                return responseDto;

            }

        }
    }
}