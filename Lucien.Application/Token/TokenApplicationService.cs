using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Application.Contracts.Sessions.Interfaces;
using Lucien.Application.Contracts.Token.Dtos;
using Lucien.Application.Contracts.Token.Interfaces;
using Lucien.Application.Contracts.Users.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var JwtSecurityRefreshToken = GenerateRefreshToken(user.Id);

            var JwtSecurityAccessToken = GenerateAccessToken(user);

            string refreshToken = new JwtSecurityTokenHandler().WriteToken(JwtSecurityRefreshToken);
            string accessToken = new JwtSecurityTokenHandler().WriteToken(JwtSecurityAccessToken);

            // Store new refresh token in DB need 
            CreateSessionDto createSessionDto = GenerateCreateSessionDto(user.Id, refreshToken);
            await _sessionApplicationService.CreateAsync(createSessionDto);

            return new TokenDto
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken,
                ExpiresAt = JwtSecurityAccessToken.ValidTo,
                RefreshTokenExpiresAt = JwtSecurityRefreshToken.ValidTo
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

        private JwtSecurityToken GenerateRefreshToken(Guid userId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),
            };

            return GenerateToken(claims, expiryMinutes);
        }

        private JwtSecurityToken GenerateAccessToken(UserDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role ?? "User"),
                new Claim("email", user.Email ??"")
            };

            return GenerateToken(claims, expiryMinutes);
        }

        private JwtSecurityToken GenerateToken(List<Claim> claims, int expiryMinutes)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
