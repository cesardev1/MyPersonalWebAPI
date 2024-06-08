using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;


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
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }

    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
    }

}
