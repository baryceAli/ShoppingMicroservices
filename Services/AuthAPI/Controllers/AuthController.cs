using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.Services.AuthAPI.Models.Dto;
using ShoppingMicroservices.Services.AuthAPI.Service.IService;

namespace ShoppingMicroservices.Services.AuthAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDto _responseDto;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto registerationRequestDto)
        {
            var errorMessage = await _authService.Register(registerationRequestDto);
            if (string.IsNullOrEmpty(errorMessage))
            {

                return Ok(_responseDto);
            }
            _responseDto.isSuccess = false;
            _responseDto.Message = errorMessage;
            return BadRequest(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var loginResponseDto = await _authService.Login(loginRequestDto);
            if (loginResponseDto.UserDto == null)
            {
                _responseDto.isSuccess = false;
                _responseDto.Message = "Invalid username or password";

                return BadRequest(_responseDto);
            }
            _responseDto.Data = loginResponseDto;
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegisterationRequestDto registerationRequestDto)
        {
            var isRoleAssigned = await _authService.AssignRole(registerationRequestDto.Email, registerationRequestDto.RoleName);
            if (!isRoleAssigned)
            {
                _responseDto.isSuccess = false;
                _responseDto.Message = $"Couldn't assign the role: {registerationRequestDto.RoleName}";

                return BadRequest(_responseDto);
            }
            _responseDto.Data = isRoleAssigned;
            return Ok(_responseDto);
        }

    }
}