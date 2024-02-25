using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace MyPersonalWebAPI.Models
{
    public class Roles
    {
        /// <summary>
        /// Model of manage rol
        /// </summary>
        /// <remarks>
        /// Permissions: will be developer later
        /// </remarks>
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
