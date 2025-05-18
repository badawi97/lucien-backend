using Lucien.Domain.Users.Entities;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lucien.Infrastructure.Repositories.Users
{
    public class UserReposittory : IUserRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;

        public UserReposittory(LucienDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<List<User>> GetListAsync(
            string? userName,
            DateTime? DateOfBirth,
            string? phone,
            int? gender,
            string? email,
            int skipCount,
            int maxResultCount,
            string? sorting
            )
        {
            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(user => user.UserName != null ? user.UserName.Contains(userName) : true);
            }

            if (DateOfBirth.HasValue)
            {
                query = query.Where(user => user.DateOfBirth != null ? user.DateOfBirth.Value.Date == DateOfBirth.Value.Date : true);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(user => user.Phone != null ? user.Phone.Contains(phone) : true);
            }

            if (gender.HasValue)
            {
                query = query.Where(user => user.Gender == gender.Value);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(user => user.Email != null ? user.Email.Contains(email) : true);
            }

            query = sorting switch
            {
                "name" => query.OrderBy(c => c.UserName),
                "name_desc" => query.OrderByDescending(c => c.UserName),
                "dateOfBirth" => query.OrderBy(c => c.DateOfBirth),
                "dateOfBirth_desc" => query.OrderByDescending(c => c.DateOfBirth),
                _ => query.OrderBy(c => c.Id)
            };

            query = query.Skip(skipCount).Take(maxResultCount);

            return await query.ToListAsync();

        }

        public async Task<List<User>> GetDeletedAsync()
        {
            return await _context.Users.IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(Guid id, User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingUser = await GetByIdAsync(id);
                User updatedUser = existingUser.Update(user);
                _context.Users.Update(updatedUser);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return updatedUser;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await GetByIdAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id)
                   ?? throw new KeyNotFoundException($"User with Id {id} was not found.");
        }

        public async Task<User> GetByEmailAsync(string? email)
        {
            if (email == null) throw new ArgumentNullException("email");

            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email)
                   ?? throw new KeyNotFoundException($"User with email {email} was not found.");
        }
    }
}
