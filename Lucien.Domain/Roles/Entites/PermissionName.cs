namespace Lucien.Domain.Roles.Entites
{
    public sealed class PermissionName
    {
        public string Value { get; } = null!;

        private PermissionName() { } // EF

        public PermissionName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Permission name required.");
            Value = value.Trim();
        }

        public override string ToString() => Value;
        public override bool Equals(object? obj) => obj is PermissionName other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }

}
