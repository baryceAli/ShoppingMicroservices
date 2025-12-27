namespace ShoppingMicroservices.FrontEnd.Web.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeaderDto { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
        // public double CartTotal { get; set; }
    }
}