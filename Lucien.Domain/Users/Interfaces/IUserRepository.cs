using Lucien.Domain.Comman.Interfaces;
using Lucien.Domain.Users.Entities;

namespace Lucien.Domain.Users.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string? email);
    }
}
