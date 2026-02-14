public class RegisterUserDto
{
    public string username { get; set;}
    public string email { get; set;}
    public string password { get; set;}
    public string confirm_password {get; set;}
}

public class LoginUserDto
{
    public string username {get; set;}
    public string password { get; set;}
}