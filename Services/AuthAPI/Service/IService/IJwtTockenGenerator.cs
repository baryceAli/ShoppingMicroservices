using ShoppingMicroservices.Services.AuthAPI.Models;

namespace ShoppingMicroservices.Services.AuthAPI.Service.IService
{
    public interface IJwtTockenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}