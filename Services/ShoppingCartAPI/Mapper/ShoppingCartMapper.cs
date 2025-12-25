using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Mapper
{

    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class ShoppingCartMapper
    {


        public static partial CartHeaderDto MapCartHeaderToDto(CartHeader cartHeader);
        public static partial IEnumerable<CartHeaderDto> MapCartHeaderToDto(IEnumerable<CartHeader> cartHeaders);


        public static partial CartHeader MapDtoToCartHeader(CartHeaderDto shoppingCartDto);
        public static partial IEnumerable<CartHeader> MapDtoToCartHeader(IEnumerable<CartHeaderDto> cartHeaderDtos);




        public static partial CartDetailsDto MapCartDetailsToDto(CartDetails cartDetails);
        public static partial IEnumerable<CartDetailsDto> MapCartDetailsToDto(IEnumerable<CartDetails> cartDetails);


        public static partial CartDetails MapDtoToCartDetails(CartDetailsDto cartDetails);
        public static partial IEnumerable<CartDetails> MapDtoToCartDetails(IEnumerable<CartDetailsDto> cartDetailsDtos);


    }
}