using Domain.Entities;

namespace Domain.Constants
{
    public static class RoleConfiguration
    {
        public static readonly string Deliverer = "Deliverer";
        public static readonly string Manager = "Manager";
        public static readonly string Administrator = "Administrator";
        public static readonly string Accountant = "Accountant";
        public static readonly string PublicUser = "PublicUser";
        public static readonly string Reader = "Reader";

        public static List<Permission> GetDelivererPermissions() =>
            new()
            {
            new Permission { Name = "deliveries.view" },
            new Permission { Name = "deliveries.update" }
            };
            
        public static List<Permission> GetManagerPermissions() =>
        new()
        {
            new Permission { Name = "users.view" },
            new Permission { Name = "users.create" },
            new Permission { Name = "reports.view" }
        };
        // ... autres méthodes de configuration
    }
}