using System.ComponentModel.DataAnnotations.Schema;

namespace tasksApi
{
    public class Role
    {
        public int id { get; set;}
        public required string name { get; set;}
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public virtual ICollection<RolePermissions>? role_permissions { get; set;}
    }
    /// <summary>
    /// name: Name of the permision ex: Add Users
    /// codename: Code of the permission, the api will look at the codename of the role to grant access, ex: add_user
    /// </summary>
    public class Permission
    {
        public int id { get; set;}
        public required string name { get; set;}
        public required string codename {get ; set;}
        public DateTime created_at { get; set;} = DateTime.UtcNow;

        public virtual ICollection<RolePermissions>? role_permissions { get; set;}
    }

    public class RolePermissions
    {
        public int id { get; set;}
        [ForeignKey("Role")]
        public int role_id { get; set;}
        public virtual Role? role { get; set;}

        [ForeignKey("Permission")]
        public int permission_id { get; set;}
        public virtual Permission? permission { get; set;}

    }
}