using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
public static class User
{
    public static void MapUserRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("/{id}", GetUser).RequireAuthorization().WithOpenApi();
        group.MapPost("/register", RegisterUser);
        group.MapPost("/login", LoginUser);
    }


    public static async Task<IResult> GetUser(int id, AppDbContext db)
    {
        var user = await db.User.FindAsync(id);
        return Results.Ok(user);
    }

    public static async Task<IResult> RegisterUser(RegisterUserDto payload, AppDbContext db)
    {
        var middleware = new UserMiddleware();
        var exists = await db.User.SingleOrDefaultAsync(u => u.username == payload.username);

        if (exists != null)
        {
            return Results.Problem("Username already exists");
        }

        if (payload.password != payload.confirm_password)
        {
            return Results.Problem("Passwords does not match");
        }


        var password_hash = middleware.HashPassword(payload.password);

        var user = new UserModel
        {
            username = payload.username,
            email = payload.email,
            password = password_hash,
            role_id = payload.role_id //the frontend should automatically sets the role_id by default (ex: the frontend sends the role_id 2 assuming its the "user" role)
        };

        await db.User.AddAsync(user);
        await db.SaveChangesAsync();

        return Results.Ok("User was created succesfully");
    }

    public static async Task<IResult> LoginUser(LoginUserDto payload, AppDbContext db)
    {
        var middleware = new UserMiddleware();
        var user = await db.User.SingleOrDefaultAsync(u => u.username == payload.username);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        if(!middleware.VerifyPassword(payload.password, user.password))
        {
            return Results.Unauthorized();
        }

        var token = middleware.GenerateJwtToken(user.username);
        return Results.Ok(new { token });
    }
}