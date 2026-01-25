public class UserModel
{
    public int id { get; set; }
    public string username { get; set;}
    public string email { get; set;}
    public string password { get; set;}
    public bool is_active {get; set; } = true;
    public DateTime created_at {get; set;} = DateTime.UtcNow;
}