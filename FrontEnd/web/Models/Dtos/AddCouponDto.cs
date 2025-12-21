namespace ShoppingMicroservices.FrontEnd.Web.Models.Dtos
{

    public class AddCouponDto
    {
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}