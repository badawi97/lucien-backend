namespace Lucien.Application.Contracts.Sessions.Dto
{
    public class CreateSessionDto
    {
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
