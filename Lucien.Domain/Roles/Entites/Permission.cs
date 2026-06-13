using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Roles.Entites
{
    public class Permission : AuditableEntityBase
    {
        public PermissionName Name { get; private set; } = default!;
        public Guid RoleId { get; private set; } = default!;
        private Permission() { } // EF
        public Permission(PermissionName name, string createdBy, Guid roleId)
        {
            Name = name;
            RoleId = roleId;
            SetAuditOnCreate(createdBy);
        }
    }
}
