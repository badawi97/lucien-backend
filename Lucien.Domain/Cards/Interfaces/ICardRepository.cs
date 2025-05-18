using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Cards.Interfaces
{
    public interface ICardRepository : ITransient
    {
        Task<List<Card>> GetListAsync(
            string? name,
            DateTime? DateOfBirth,
            string? phone,
            int? gender,
            string? email,
            int skipCount,
            int maxResultCount,
            string? sorting
            );

        Task<Card> GetByIdAsync(Guid id);
        Task<Card> CreateAsync(Card card);
        Task<Card> UpdateAsync(Guid id, Card card);
        Task DeleteAsync(Guid id);
        Task<int> GetCountAsync();
    }
}
