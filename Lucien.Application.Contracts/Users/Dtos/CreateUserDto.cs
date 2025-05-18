namespace Lucien.Application.Contracts.Users.Dtos
{
    public class CreateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
    }
}
