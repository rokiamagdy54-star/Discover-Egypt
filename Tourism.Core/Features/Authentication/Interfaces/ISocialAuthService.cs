using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Features.Authentication.DTOs;

namespace Tourism.Core.Features.Authentication.Interfaces
{
    public interface ISocialAuthService
    {
        Task<UserInfoDto> VerifyTokenAsync(string token, string provider);
    }
}
