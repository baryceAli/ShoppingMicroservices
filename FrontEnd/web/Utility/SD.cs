namespace ShoppingMicroservices.FrontEnd.Web.Utility
{
    public static class SD
    {
        public static string CouponAPIBase { get; set; } = string.Empty;
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}