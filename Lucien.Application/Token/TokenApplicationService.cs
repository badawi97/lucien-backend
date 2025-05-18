using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Application.Contracts.Sessions.Interfaces;
using Lucien.Application.Contracts.Token.Dtos;
using Lucien.Application.Contracts.Token.Interfaces;
using Lucien.Application.Contracts.Users.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lucien.Application.Token
{
    public class TokenApplicationService : ITokenApplicationService
    {

        private readonly IConfiguration _configuration;
        private readonly ISessionApplicationService _sessionApplicationService;

        public TokenApplicationService(IConfiguration configuration, ISessionApplicationService sessionApplicationService)
        {
            _configuration = configuration;
            _sessionApplicationService = sessionApplicationService;
        }

        public async Task<TokenDto> GetAsync(UserDto user)
        {
            var refreshToken = GenerateRefreshToken();

            var accessToken = GenerateAccessToken(user);

            // Store new refresh token in DB
            CreateSessionDto createSessionDto = GenerateCreateSessionDto(user.Id, refreshToken);
            await _sessionApplicationService.CreateAsync(createSessionDto);

            return new TokenDto
            {
                RefreshToken = refreshToken,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                ExpiresAt = accessToken.ValidTo
            };
        }

        private CreateSessionDto GenerateCreateSessionDto(Guid userId, string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tefreshExpiryDays = int.Parse(jwtSettings["RefreshExpiryDays"] ?? "0");

            return new CreateSessionDto
            {
                UserId = userId,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(tefreshExpiryDays)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private JwtSecurityToken GenerateAccessToken(UserDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userName ", user.UserName ?? ""),
                new Claim("role", user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return token;
        }

    }
}
