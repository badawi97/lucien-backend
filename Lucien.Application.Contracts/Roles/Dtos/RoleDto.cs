namespace Lucien.Application.Contracts.Roles.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }
    }
}
