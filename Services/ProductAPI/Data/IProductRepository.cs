
using ShoppingMicroservices.Services.ProductAPI.Models;

namespace ShoppingMicroservices.Services.ProductAPI.Data
{
    public interface IProductRepository
    {
        public IEnumerable<Product> GetProducts();
        public Product? GetProductById(int id);
        // public Coupon? GetCouponByCode(string code);
        public Product AddProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(int id);
    }
}