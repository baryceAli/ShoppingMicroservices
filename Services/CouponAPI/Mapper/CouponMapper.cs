using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.Services.CouponAPI.Models;
using ShoppingMicroservices.Services.CouponAPI.Models.Dtos;

namespace ShoppingMicroservices.Services.CouponAPI.Mapper
{
    // Enums of source and target have different numeric values -> use ByName strategy to map them
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class CouponMapper
    {

        // [MapProperty(nameof(coupon.CouponId), nameof(CouponDto.CouponId))]
        public static partial CouponDto MapCouponToDto(Coupon coupon);
        public static partial IEnumerable<CouponDto> MapCouponToDto(IEnumerable<Coupon> coupons);


        // public static partial Coupon MapDtoToCoupon(CouponDto couponDto);
        // public static partial IEnumerable<Coupon> MapDtoToCoupon(IEnumerable<CouponDto> couponDtos);

        public static partial Coupon MapAddCouponDtoToCoupon(AddCouponDto addCouponDto);

    }
}