namespace MyPersonalWebAPI.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    public class RolePermission
    {
        public int RoleId { get; set; }
        public Roles RoleId { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}