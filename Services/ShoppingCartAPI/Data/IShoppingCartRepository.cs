
using ShoppingMicroservices.Services.ProductAPI.Models;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Data
{
    public interface IShoppingCartRepository
    {
        public IEnumerable<ShoppingCart> GetShoppingCarts();
        public ShoppingCart? GetShoppingCartById(int id);
        // public Coupon? GetCouponByCode(string code);
        public ShoppingCart AddShoppingCart(ShoppingCart shoppingCart);
        public void UpdateShoppingCart(ShoppingCart shoppingCart);
        public void DeleteShoppingCart(int id);
    }
}