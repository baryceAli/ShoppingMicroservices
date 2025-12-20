using ShoppingMicroservices.Services.CouponAPI.Data;
using ShoppingMicroservices.Services.CouponAPI.Models;

namespace ShoppingMicroservices.Data
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            this._context = context;
        }
        public Coupon AddCoupon(Coupon coupon)
        {
            var res = _context.Coupons.Add(coupon);
            _context.SaveChanges();
            return res.Entity;
        }

        public void DeleteCoupon(int id)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.CouponId == id);
            if (coupon != null)
            {

                _context.Coupons.Remove(coupon);
                _context.SaveChanges();
            }
        }

        public Coupon? GetCouponByCode(string code)
        {
            return _context.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());
        }

        public Coupon? GetCouponById(int id)
        {
            return _context.Coupons.FirstOrDefault(c => c.CouponId == id);
        }

        public IEnumerable<Coupon> GetCoupons()
        {
            return _context.Coupons.ToList();
        }

        public void UpdateCoupon(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            _context.SaveChanges();
        }
    }
}