namespace Lucien.Domain.Shared.Authorization
{
    public static class PermissionNames
    {
        public const string UsersRead = "Users.Read";
        public const string UsersCreate = "Users.Create";
        public const string UsersUpdate = "Users.Update";
        public const string UsersDelete = "Users.Delete";

        public const string CardsRead = "Cards.Read";
        public const string CardsCreate = "Cards.Create";
        public const string CardsUpdate = "Cards.Update";
        public const string CardsDelete = "Cards.Delete";
        public const string CardsExport = "Cards.Export";
        public const string CardsImport = "Cards.Import";

        public const string RolesRead = "Roles.Read";
        public const string RolesCreate = "Roles.Create";
        public const string RolesUpdate = "Roles.Update";
        public const string RolesDelete = "Roles.Delete";

        public const string PermissionsRead = "Permissions.Read";
        public const string PermissionsCreate = "Permissions.Create";
        public const string PermissionsUpdate = "Permissions.Update";
        public const string PermissionsDelete = "Permissions.Delete";

        public static readonly string[] All =
        {
            UsersRead,
            UsersCreate,
            UsersUpdate,
            UsersDelete,
            CardsRead,
            CardsCreate,
            CardsUpdate,
            CardsDelete,
            CardsExport,
            CardsImport,
            RolesRead,
            RolesCreate,
            RolesUpdate,
            RolesDelete,
            PermissionsRead,
            PermissionsCreate,
            PermissionsUpdate,
            PermissionsDelete
        };
    }
}
