namespace Lucien.Application.Contracts.Permissions.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string? Name { get; set; }
    }
}
