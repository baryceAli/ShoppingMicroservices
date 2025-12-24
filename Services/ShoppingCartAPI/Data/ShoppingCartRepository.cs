




using ShoppingMicroservices.Services.ShoppingCartAPI.Models;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context)
        {
            this._context = context;
        }
        public ShoppingCart AddShoppingCart(ShoppingCart shoppingCart)
        {
            var res = _context.ShoppingCarts.Add(shoppingCart);
            _context.SaveChanges();
            return res.Entity;
        }

        public void DeleteShoppingCart(int id)
        {
            var shoppingCart = _context.ShoppingCarts.FirstOrDefault(p => p.ShoppingCartId == id);
            if (shoppingCart != null)
            {

                _context.ShoppingCarts.Remove(shoppingCart);
                _context.SaveChanges();
            }
        }

        // public ShoppingCart? GetCouponByCode(string code)
        // {
        //     return _context.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
        // }

        public ShoppingCart? GetShoppingCartById(int id)
        {
            return _context.ShoppingCarts.FirstOrDefault(c => c.ShoppingCartId == id);
        }

        public IEnumerable<ShoppingCart> GetShoppingCarts()
        {
            return _context.ShoppingCarts.ToList();
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Update(shoppingCart);
            _context.SaveChanges();
        }
    }
}