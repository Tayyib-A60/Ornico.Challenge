using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service.BL.Interfaces;
using System.Threading.Tasks;
using Models.DTOs;
using Models.ViewModels;
using System.Security.Claims;

namespace Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(BaseResponse<UserLoginResponse>), 200)]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDTO createUserDTO)
        {
            var response = await _authService.CreateUser(createUserDTO);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<UserLoginResponse>), 200)]
        public async Task<IActionResult> Login([FromBody] AuthUserDTO authUserDTO)
        {
            var response = await _authService.Authenticate(authUserDTO);
            return Ok(response);
        }

        [HttpPost("verify-account")]
        [ProducesResponseType(typeof(BaseResponse<EmailVerificationResponse>), 200)]
        public async Task<IActionResult> VerifyAccount([FromBody] EmailVerificationDTO emailVerificationDTO)
        {
            var response = await _authService.VerifyEmail(emailVerificationDTO, GetUserId());
            return Ok(response);
        }

        [HttpPost("send-verification-email")]
        [ProducesResponseType(typeof(BaseResponse<EmailVerificationResponse>), 200)]
        public async Task<IActionResult> SendVerificationEmail()
        {
            var response = await _authService.SendVerificationEmail(GetUserId());
            return Ok(response);
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
