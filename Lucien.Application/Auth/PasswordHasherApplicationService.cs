using Lucien.Application.Contracts.Auth.Interfaces;

namespace Lucien.Application.Auth
{
    public class PasswordHasherApplicationService : IPasswordHasherApplicationService
    {
        public string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashedPassword(string? hashedPassword, string? providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
