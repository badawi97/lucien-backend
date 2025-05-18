using Lucien.Domain.Shared.DI;

namespace Lucien.Application.Contracts.Auth.Interfaces
{
    public interface IPasswordHasherApplicationService : ITransient
    {
        string HashPassword(string? password);
        bool VerifyHashedPassword(string? hashedPassword, string? providedPassword);
    }
}
