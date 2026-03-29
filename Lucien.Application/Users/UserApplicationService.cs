using AutoMapper;
using Lucien.Application.Contracts.Auth.Interfaces;
using Lucien.Application.Contracts.Common.Dto;
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

        public async Task<PagedResultDto<UserDto>> GetListAsync(PagedRequestUserDto input)
        {
            // 1. Start from the base query (DbSet<User>)
            IQueryable<User> baseQuery = _userRepository.GetQueryable();

            // 2. Apply any filters (e.g., only active users)
            // baseQuery = baseQuery.Where(u => u.IsActive); // optional filter example            
            if (input.DateOfBirth.HasValue)
            {
                baseQuery = baseQuery.Where(user => user.DateOfBirth != null ? user.DateOfBirth.Value.Date == input.DateOfBirth.Value.Date : true);
            }

            if (!string.IsNullOrEmpty(input.Phone))
            {
                baseQuery = baseQuery.Where(user => user.Phone != null ? user.Phone.Contains(input.Phone) : true);
            }

            if (input.Gender.HasValue)
            {
                baseQuery = baseQuery.Where(user => user.Gender == input.Gender.Value);
            }

            if (!string.IsNullOrEmpty(input.Email))
            {
                baseQuery = baseQuery.Where(user => user.Email != null ? user.Email.Contains(input.Email) : true);
            }

            // 3. Apply sorting if specified, else default sorting
            Func<IQueryable<User>, IQueryable<User>>? sortingFunc = null;
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                // Example: use System.Linq.Dynamic.Core or build expression dynamically
                switch (input.Sorting)
                {
                    case "Email":
                        sortingFunc = q => q.OrderBy(e => e.Email);
                        break;
                    case "Gender":
                        sortingFunc = q => q.OrderBy(e => e.Gender);
                        break;
                    case "DateOfBirth":
                        sortingFunc = q => q.OrderBy(e => e.DateOfBirth);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Default sorting
                sortingFunc = q => q.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
            }

            var pagedResult = await _userRepository.GetListAsync(baseQuery, input.SkipCount, input.MaxResultCount, sortingFunc);

            var dtoList = _mapper.Map<List<UserDto>>(pagedResult.Items);
            return new PagedResultDto<UserDto>(pagedResult.TotalCount, dtoList);
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

        public async Task<UserDto?> GetByEmailAsync(string? email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            return user == null ? null : _mapper.Map<UserDto>(user);
        }
    }
}
