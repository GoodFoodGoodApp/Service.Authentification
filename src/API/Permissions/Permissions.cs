namespace API.Permissions;

public static class Permissions
{
    public const string AdminAccess = "admin_access";
    public const string ClientAccess = "client_access";
    public const string ManagerAccess = "manager_access";
    public const string DeliveryAccess = "delivery_access";


    public const string AdminRole = "admin";
    public const string ManagerRole = "manager";
    public const string DeliveryRole = "delivery";
    public const string ClientRole = "client";

    public static List<string> AllRoles => new List<string>
    {
        AdminRole,
        ManagerRole,
        DeliveryRole,
        ClientRole
    };
    public static class Admin
    {
        public const string CreateUser = "admin.create_user";
        public const string DeleteUser = "admin.delete_user";
        public const string ManageRoles = "admin.manage_roles";
        public const string ViewAllUsers = "admin.view_all_users";

        public static List<string> AllPermissions => new List<string>
        {
            CreateUser,
            DeleteUser,
            ManageRoles,
            ViewAllUsers
        };
    }

    public static class Manager
    {
        public const string ManageOrders = "manager.manage_orders";
        public const string ViewStatistics = "manager.view_statistics";
        public const string ManageProducts = "manager.manage_products";

        public static List<string> AllPermissions => new List<string>
        {
            ManageOrders,
            ViewStatistics,
            ManageProducts
        };
    }

    public static class Client
    {
        public const string PlaceOrder = "client.place_order";
        public const string ViewOwnOrders = "client.view_own_orders";
        public const string UpdateProfile = "client.update_profile";

        public static List<string> AllPermissions => new List<string>
        {
            PlaceOrder,
            ViewOwnOrders,
            UpdateProfile
        };
    }

    public static class Delivery
    {
        public const string ViewAssignedDeliveries = "delivery.view_assigned";
        public const string UpdateDeliveryStatus = "delivery.update_status";

        public static List<string> AllPermissions => new List<string>
        {
            ViewAssignedDeliveries,
            UpdateDeliveryStatus
        };
    }
}
