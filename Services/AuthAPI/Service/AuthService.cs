using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingMicroservices.Services.AuthAPI.Data;
using ShoppingMicroservices.Services.AuthAPI.Models;
using ShoppingMicroservices.Services.AuthAPI.Models.Dto;
using ShoppingMicroservices.Services.AuthAPI.Service.IService;

namespace ShoppingMicroservices.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTockenGenerator _jwtTockenGenerator;

        public AuthService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTockenGenerator jwtTockenGenerator)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._jwtTockenGenerator = jwtTockenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var isRoleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!isRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);
            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { UserDto = null, Token = string.Empty };
            }

            //Generate JwtToken

            UserDto userDto = new()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            var roles = await _userManager.GetRolesAsync(user);
            LoginResponseDto loginResponseDto = new()
            {
                UserDto = userDto,
                Token = _jwtTockenGenerator.GenerateToken(user, roles)
            };

            return loginResponseDto;

        }

        public async Task<string> Register(RegisterationRequestDto registerationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDto.Email,
                Email = registerationRequestDto.Email,
                NormalizedEmail = registerationRequestDto.Email.ToUpper(),
                Name = registerationRequestDto.Name,
                PhoneNumber = registerationRequestDto.PhoneNumber,

            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = await _context.ApplicationUsers.FirstAsync(u => u.UserName == registerationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return string.Empty;
                }
                return result.Errors.FirstOrDefault().Description;
            }
            catch (System.Exception ex)
            {
                return ex.Message;

                // throw;
            }
            // throw new NotImplementedException();
        }
    }
}