using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

public class UserMiddleware
{
    public bool VerifyPassword(string password, string storedHash)
    {
        // Extract the salt and hash from the stored hash
        var parts = storedHash.Split(':');
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        // Compute the hash of the provided password
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            var computedHash = Convert.ToBase64String(pbkdf2.GetBytes(32)); // 32 bytes = 256 bits
            return computedHash == hash;
        }
    }

    public string HashPassword(string password)
    {
        // Generate a random salt
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Compute the hash of the password with the salt
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            var hash = Convert.ToBase64String(pbkdf2.GetBytes(32)); // 32 bytes = 256 bits
            return $"{Convert.ToBase64String(salt)}:{hash}";
        }
    }

    public string GenerateJwtToken(string username)
    {
        Env.Load();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTH_KEY")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com", //just as a example
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}