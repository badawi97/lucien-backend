namespace Lucien.Application.Contracts.Sessions.Dto
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
