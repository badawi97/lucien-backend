using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Roles.Entites
{
    public class Role : AuditableAggregateRoot<Role>
    {
        private readonly List<Permission> _permissions = new();
        public string Name { get; private set; } = default!;
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

        private Role() { } // EF

        public Role(string name, string createdBy)
        {
            Rename(name, createdBy); // validates + audit
            AddDomainEvent(new RoleCreated(Id, Name));
        }

        public void Rename(string newName, string actor)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Role name required.");
            Name = newName.Trim();
            if (CreatedAt == default) SetAuditOnCreate(actor); else SetAuditOnUpdate(actor);
            AddDomainEvent(new RoleRenamed(Id, Name));
        }

        public void AddPermission(Permission permission, string actor)
        {
            if (_permissions.Any(p => p.Name.Equals(permission.Name)))
                throw new InvalidOperationException("Permission already exists.");
            _permissions.Add(permission);
            SetAuditOnUpdate(actor);
            AddDomainEvent(new PermissionAssignedToRole(Id, permission.Name.ToString()));
        }

        public void RemovePermission(PermissionName name, string actor)
        {
            var p = _permissions.FirstOrDefault(x => x.Name.Equals(name));
            if (p is null) throw new InvalidOperationException("Permission not found.");
            _permissions.Remove(p);
            SetAuditOnUpdate(actor);
            AddDomainEvent(new PermissionRemovedFromRole(Id, name.ToString()));
        }

        public override Role Update(Role entity)
        {
            throw new NotImplementedException();
        }
    }

    // Domain/Roles/Events/...
    public record RoleCreated(Guid RoleId, string Name) : DomainEventBase;
    public record RoleRenamed(Guid RoleId, string NewName) : DomainEventBase;
    public record PermissionAssignedToRole(Guid RoleId, string PermissionName) : DomainEventBase;
    public record PermissionRemovedFromRole(Guid RoleId, string PermissionName) : DomainEventBase;
}
