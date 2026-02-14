using System.ComponentModel.DataAnnotations.Schema;
using tasksApi;

public class UserModel
{
    public int id { get; set; }
    public required string username { get; set;}
    public required string email { get; set;}
    public required string password { get; set;}
    public bool is_active {get; set; } = true;
    [ForeignKey("Role")]
    public int role_id { get; set;}
    public virtual Role? role { get; set;}
    public DateTime created_at {get; set;} = DateTime.UtcNow;
}