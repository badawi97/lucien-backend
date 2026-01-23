using Lucien.Domain.Shared.Entities;

namespace Lucien.Domain.Sessions.Entities
{
    public class Session : AuditableEntity<Session>
    {
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

        public override Session Update(Session entity)
        {
            throw new NotImplementedException();
        }
    }
}
