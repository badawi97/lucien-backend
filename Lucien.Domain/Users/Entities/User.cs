using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Users.Entities
{
    public class User : AuditableEntity<User>
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public int? Gender { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Photo { get; private set; }
        public string? Address { get; private set; }
        public string? PasswordHash { get; set; }

        public override User Update(User updatedUser)
        {

            if (updatedUser.DateOfBirth.HasValue)
            {
                DateOfBirth = updatedUser.DateOfBirth.Value;
            }

            if (!string.IsNullOrEmpty(updatedUser.Phone))
            {
                Phone = updatedUser.Phone;
            }

            if (updatedUser.Gender.HasValue)
            {
                Gender = updatedUser.Gender.Value;
            }

            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                Email = updatedUser.Email;
            }

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                PasswordHash = updatedUser.PasswordHash;
            }
            return this;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            Email = email;
        }

        public void UpdatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be empty", nameof(phone));
            Phone = phone;
        }
    }
}
