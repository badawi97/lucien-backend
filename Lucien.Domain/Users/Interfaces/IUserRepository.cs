using Lucien.Domain.Shared.DI;
using Lucien.Domain.Users.Entities;

namespace Lucien.Domain.Users.Interfaces
{
    public interface IUserRepository : ITransient
    {
        Task<List<User>> GetListAsync(
          string? userName,
          DateTime? DateOfBirth,
          string? phone,
          int? gender,
          string? email,
          int skipCount,
          int maxResultCount,
          string? sorting
          );
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string? email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(Guid id, User user);
        Task DeleteAsync(Guid id);
        Task<List<User>> GetDeletedAsync();
        Task<int> GetCountAsync();
    }
}
