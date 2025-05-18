namespace Lucien.Application.Contracts.Users.Dtos
{
    public class UserDto
    {
        public Guid Id { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? UserName { get; private set; }
        public int? Gender { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Photo { get; private set; }
        public string? Address { get; private set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }

    }
}
