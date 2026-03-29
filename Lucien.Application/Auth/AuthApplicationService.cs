using Lucien.Application.Contracts.Auth.Dtos;
using Lucien.Application.Contracts.Auth.Interfaces;
using Lucien.Application.Contracts.Sessions.Interfaces;
using Lucien.Application.Contracts.Token.Dtos;
using Lucien.Application.Contracts.Token.Interfaces;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Application.Contracts.Users.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Users.Interfaces;

namespace Lucien.Application.Auth
{
    public class AuthApplicationService : IAuthApplicationService
    {
        private readonly IUserApplicationService _userApplicationService;
        private readonly ISessionApplicationService _sessionApplicationService;
        private readonly ITokenApplicationService _tokenApplicationService;
        private readonly IPasswordHasherApplicationService _passwordHasherApplicationService;

        public AuthApplicationService(
            IUserApplicationService userApplicationService,
            ITokenApplicationService tokenApplicationService,
            IPasswordHasherApplicationService passwordHasherApplicationService,
            ISessionApplicationService sessionApplicationService)
        {
            _userApplicationService = userApplicationService;
            _tokenApplicationService = tokenApplicationService;
            _passwordHasherApplicationService = passwordHasherApplicationService;
            _sessionApplicationService = sessionApplicationService;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            UserDto? user = await _userApplicationService.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User Not Found");
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new UnauthorizedAccessException();
            }
            if (!_passwordHasherApplicationService.VerifyHashedPassword(user.PasswordHash, loginDto.Password))
            {
                throw new UnauthorizedAccessException();
            }

            return await _tokenApplicationService.GetAsync(user);
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            // validate existing user
            UserDto? existingUser = await _userApplicationService.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already exists");

            // hash password
            string passwordHash = _passwordHasherApplicationService.HashPassword(registerDto.Password);

            // create user
            CreateUserDto user = new CreateUserDto
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = registerDto.Role,
                Phone = registerDto.Phone,
            };

            UserDto createdUser = await _userApplicationService.CreateAsync(user);

            // generate tokens
            return await _tokenApplicationService.GetAsync(createdUser);
        }

        public async Task<TokenDto> GetRefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException();

            var session = await _sessionApplicationService.GetByRefreshTokenAsync(refreshToken);
            if (session == null)//|| session.IsExpired || session.IsRevoked
                throw new UnauthorizedAccessException();

            var user = await _userApplicationService.GetAsync(session.UserId);

            return await _tokenApplicationService.GetAsync(user);
        }
    }
}
