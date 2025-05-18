using Lucien.Domain.Shared.DI;

namespace Lucien.Domain.Shared.Interfaces
{
    public interface IUserContextService : IScoped
    {
        string GetCurrentUserId();
    }
}
