using ShoppingMicroservices.Services.CouponAPI.Models;

namespace ShoppingMicroservices.Data
{
    public interface ICouponRepository
    {
        public IEnumerable<Coupon> GetCoupons();
        public Coupon? GetCouponById(int id);
        public Coupon? GetCouponByCode(string code);
        public Coupon AddCoupon(Coupon coupon);
        public void UpdateCoupon(Coupon coupon);
        public void DeleteCoupon(int id);
    }
}