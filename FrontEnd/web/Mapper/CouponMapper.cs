

using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;

namespace ShoppingMicroservices.FrontEnd.Mapper
{
    // Enums of source and target have different numeric values -> use ByName strategy to map them
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class CouponMapper
    {

        // [MapProperty(nameof(coupon.CouponId), nameof(CouponDto.CouponId))]
        public static partial AddCouponDto MapCouponDtoToAddCouponDto(CouponDto couponDto);


    }
}