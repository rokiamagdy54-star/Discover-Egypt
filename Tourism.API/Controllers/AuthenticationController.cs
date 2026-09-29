using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.Authentication.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Creates a new user account.</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            return StatusCode(201, result);
        }

        /// <summary>Authenticates a user and returns tokens.</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            return Ok(result);
        }

        /// <summary>Authenticates via Google or Facebook.</summary>
        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin([FromBody] SocialLoginDto model)
        {
            var result = await _authService.SocialLogin(model);
            return Ok(result);
        }

        /// <summary>Generates a new access token using a valid refresh token.</summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }

        /// <summary>Revokes a specific refresh token.</summary>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
        {
            await _authService.RevokeTokenAsync(refreshToken);
            return Ok(new { message = "Token revoked successfully" });
        }

        /// <summary>Ends the current session and revokes all active refresh tokens.</summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _authService.LogoutAsync(userId!);
            return Ok(new { message = "Logged out successfully" });
        }

        /// <summary>Sends an OTP to the user's email for password reset.</summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            await _authService.SendOtpAsync(email);
            return Ok(new { message = "If this email is registered, an OTP has been sent" });
        }

        /// <summary>Resets the user's password using a valid OTP.</summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetWithOtpAsync(dto);

            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "Password reset successfully" });
        }
    }
}