using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Sessions.Entities
{
    public class Session : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
