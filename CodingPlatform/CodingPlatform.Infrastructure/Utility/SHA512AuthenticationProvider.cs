using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using CodingPlatform.Domain.Interfaces.Utility;
using Microsoft.IdentityModel.Tokens;

namespace CodingPlatform.Infrastructure.Utility
{
    public class SHA512AuthenticationProvider : IAuthenticationProvider
    {
        public SHA512AuthenticationProvider()
        {
        }

        public (byte[] Key, byte[] ComputedHash) HashPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));

            using var hmac = new HMACSHA512();

            return new(
                hmac.Key,
                hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainText)));
        }

        public bool VerifyPassword(string plainTextPassword, byte[] salt, byte[] hashPassword)
        {
            using var hmac = new HMACSHA512(salt);

            return hashPassword.SequenceEqual(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainTextPassword)));
        }

        public string GenerateJWT(long userId, string email, string keyGen)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyGen));
            var cred = new SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

