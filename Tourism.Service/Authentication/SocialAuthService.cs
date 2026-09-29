using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.Authentication.Interfaces;
using Google.Apis.Auth;
using Newtonsoft.Json;

namespace Tourism.Service.Authentication
{
    public class SocialAuthService : ISocialAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SocialAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserInfoDto> VerifyTokenAsync(string token, string provider)
        {
            return provider switch
            {
                "Google" => await VerifyGoogleToken(token),
                "Facebook" => await VerifyFacebookToken(token),
                _ => null
            };
        }

        private async Task<UserInfoDto> VerifyGoogleToken(string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);

            return new UserInfoDto
            {
                Email = payload.Email,
                Name = payload.Name
            };
        }

        private async Task<UserInfoDto> VerifyFacebookToken(string token)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(
                $"https://graph.facebook.com/me?fields=name,email&access_token={token}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            return new UserInfoDto
            {
                Email = data?.email,
                Name = data?.name
            };
        }
    }
}