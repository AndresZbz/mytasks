using Microsoft.EntityFrameworkCore;

public static class Rbac
{
    public static void MapRbacRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/rbac");

        group.MapGet("/roles/", GetAllRoles);
    }

    //Roles endpoints
    public static async Task<IResult> GetAllRoles(AppDbContext db)
    {
        var roles = await db.Role.ToListAsync();
        return Results.Ok(roles);
    } 
}