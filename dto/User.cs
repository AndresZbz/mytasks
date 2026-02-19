public class RegisterUserDto
{
    public required string username { get; set;}
    public required string email { get; set;}
    public required string password { get; set;}
    public required string confirm_password {get; set;}
    public required int role_id {get; set;}
}

public class LoginUserDto
{
    public required string username {get; set;}
    public required string password { get; set;}
}