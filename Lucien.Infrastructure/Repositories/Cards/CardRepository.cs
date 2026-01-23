using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Cards.Interfaces;
using Lucien.Domain.Shared.Interfaces;
using Lucien.Infrastructure.Repositories.Common;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Cards
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        private readonly LucienDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<CardRepository> _logger;

        public CardRepository(LucienDbContext context, IUserContextService userContextService, ILogger<CardRepository> logger) : base(userContextService, context, logger)
        {
            _context = context;
            _userContextService = userContextService;
            _logger = logger;
        }
    }
}
