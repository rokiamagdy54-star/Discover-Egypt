using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Helpers
{
    public class OtpHelper
    {
        public static string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public static string HashOtp(string otp)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(otp)));
        }

        public static bool VerifyOtp(string otp, string hash)
        {
            return HashOtp(otp) == hash;
        }
    }
}
