using Lucien.Domain.Roles.Entites;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lucien.Infrastructure.Converters
{
    public sealed class PermissionNameConverter : ValueConverter<PermissionName, string>
    {
        public PermissionNameConverter() : base(
            v => v.Value,
            v => new PermissionName(v))
        { }
    }
}
