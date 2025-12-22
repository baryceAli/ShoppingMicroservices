using Microsoft.AspNetCore.Identity;

namespace ShoppingMicroservices.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}