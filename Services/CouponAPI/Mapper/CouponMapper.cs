using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.Services.CouponAPI.Models;
using ShoppingMicroservices.Services.CouponAPI.Models.Dtos;

namespace ShoppingMicroservices.Services.CouponAPI.Mapper
{
    // Enums of source and target have different numeric values -> use ByName strategy to map them
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class CouponMapper
    {
        public static partial CouponDto MapCouponToDto(Coupon coupon);

    }
}