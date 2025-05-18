using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Cards.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lucien.Infrastructure.Repositories.Cards
{
    public class CardRepository : ICardRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;

        public CardRepository(LucienDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public async Task<List<Card>> GetListAsync(
            string? name,
            DateTime? DateOfBirth,
            string? phone,
            int? gender,
            string? email,
            int skipCount,
            int maxResultCount,
            string? sorting
            )
        {
            var query = _context.Cards.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(card => card.Name != null ? card.Name.Contains(name) : true);
            }

            if (DateOfBirth.HasValue)
            {
                query = query.Where(card => card.DateOfBirth != null ? card.DateOfBirth.Value.Date == DateOfBirth.Value.Date : true);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(card => card.Phone != null ? card.Phone.Contains(phone) : true);
            }

            if (gender.HasValue)
            {
                query = query.Where(card => card.Gender == gender.Value);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(card => card.Email != null ? card.Email.Contains(email) : true);
            }

            query = sorting switch
            {
                "name" => query.OrderBy(c => c.Name),
                "name_desc" => query.OrderByDescending(c => c.Name),
                "dateOfBirth" => query.OrderBy(c => c.DateOfBirth),
                "dateOfBirth_desc" => query.OrderByDescending(c => c.DateOfBirth),
                _ => query.OrderBy(c => c.Id)
            };

            query = query.Skip(skipCount).Take(maxResultCount);

            return await query.ToListAsync();

        }

        public async Task<List<Card>> GetDeletedCardsAsync()
        {
            return await _context.Cards.IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Card> CreateAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<Card> UpdateAsync(Guid id, Card card)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingCard = await GetByIdAsync(id);
                Card updatedCard = existingCard.Update(card);
                _context.Cards.Update(updatedCard);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return updatedCard;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var card = await GetByIdAsync(id);
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Cards.CountAsync();
        }

        public async Task<Card> GetByIdAsync(Guid id)
        {
            return await _context.Cards.FindAsync(id)
                   ?? throw new KeyNotFoundException($"Card with Id {id} was not found.");
        }
    }
}
