using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Cards.Entities
{
    public class Card : AuditableEntity<Card>
    {
        public string? Name { get; private set; }
        public int? Gender { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Photo { get; private set; }
        public string? Address { get; private set; }

        public override Card Update(Card updatedCard)
        {
            if (!string.IsNullOrEmpty(updatedCard.Name))
            {
                Name = updatedCard.Name;
            }

            if (updatedCard.DateOfBirth.HasValue)
            {
                DateOfBirth = updatedCard.DateOfBirth.Value;
            }

            if (!string.IsNullOrEmpty(updatedCard.Phone))
            {
                Phone = updatedCard.Phone;
            }

            if (updatedCard.Gender.HasValue)
            {
                Gender = updatedCard.Gender.Value;
            }

            if (!string.IsNullOrEmpty(updatedCard.Email))
            {
                Email = updatedCard.Email;
            }
            return this;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            Name = name;
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
