using AutoMapper;
using Lucien.Application.Contracts.Auth.Interfaces;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Application.Contracts.Users.Interfaces;
using Lucien.Domain.Users.Entities;
using Lucien.Domain.Users.Interfaces;

namespace Lucien.Application.Users
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IPasswordHasherApplicationService _passwordHasherApplicationService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserApplicationService(IUserRepository userRepository, IMapper mapper, IPasswordHasherApplicationService passwordHasherApplicationService)
        {
            _passwordHasherApplicationService = passwordHasherApplicationService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<List<UserDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            input.PasswordHash = _passwordHasherApplicationService.HashPassword(input.Password);
            var user = _mapper.Map<User>(input);
            var createdUser = await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input)
        {
            input.PasswordHash = _passwordHasherApplicationService.HashPassword(input.Password);
            var user = _mapper.Map<User>(input);
            var updatedUser = await _userRepository.UpdateAsync(id, user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string? email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }
    }
}
