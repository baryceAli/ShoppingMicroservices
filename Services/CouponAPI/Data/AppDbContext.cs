using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingMicroservices.Services.CouponAPI.Models;

namespace ShoppingMicroservices.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}