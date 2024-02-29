using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        public int RolePermissionId { get; set; }

        [Column(Order = 1)]
        public int RoleId { get; set; }
        public Roles Role { get; set; }

        [Column(Order = 2)]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}