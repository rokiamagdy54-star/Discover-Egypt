using Tourism.Core.Features.Authentication.DTOs;

namespace Tourism.Core.Features.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
        Task<AuthResponseDto> SocialLogin(SocialLoginDto model);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);   
        Task LogoutAsync(string userId);              
        Task<bool> ResetWithOtpAsync(ResetPasswordDto dto);
        Task SendOtpAsync(string email);
    }
}