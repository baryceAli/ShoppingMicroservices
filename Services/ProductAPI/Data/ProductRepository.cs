


using ShoppingMicroservices.Services.ProductAPI.Models;

namespace ShoppingMicroservices.Services.ProductAPI.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            this._context = context;
        }
        public Product AddProduct(Product product)
        {
            var res = _context.Products.Add(product);
            _context.SaveChanges();
            return res.Entity;
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {

                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        // public Product? GetCouponByCode(string code)
        // {
        //     return _context.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
        // }

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(c => c.ProductId == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}