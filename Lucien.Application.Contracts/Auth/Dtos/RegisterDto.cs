namespace Lucien.Application.Contracts.Auth.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!; 
        public Guid? RoleId { get; set; }
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
