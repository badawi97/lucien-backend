namespace Lucien.Domain.Shared.Entities
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }

        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }

        public void SetAuditOnCreate(string? createdBy)
        {
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetAuditOnUpdate(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete(string deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }

}
