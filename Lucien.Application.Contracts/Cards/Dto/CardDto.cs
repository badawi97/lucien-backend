
namespace Lucien.Application.Contracts.Cards.Dto
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        public string? Address { get; set; }
    }
}
