using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Comman.Interfaces;
using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Cards.Interfaces
{
    public interface ICardRepository : IRepository<Card>, ITransient
    {

    }
}
