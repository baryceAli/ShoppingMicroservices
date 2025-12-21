namespace ShoppingMicroservices.FrontEnd.Web.Models.Dtos
{
    public class ResponseDto
    {
        public object? Data { get; set; }
        public bool isSuccess { get; set; } = true;
        public string? Message { get; set; } = string.Empty;
    }
}