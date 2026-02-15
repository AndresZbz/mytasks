using Microsoft.EntityFrameworkCore;

public static class Rbac
{
    public static void MapRbacRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/rbac");

        //roles
        group.MapGet("/roles/", GetAllRoles);

        //permissions
        group.MapGet("/permissions/", GetAllPermissions);

        //role_permissions
        group.MapGet("/role_permissions/", GetAllRolePermissions);
    }

    //Roles endpoints
    public static async Task<IResult> GetAllRoles(AppDbContext db)
    {
        var roles = await db.Role.ToListAsync();
        return Results.Ok(roles);
    }

    //Permissions endopints

    public static async Task<IResult> GetAllPermissions(AppDbContext db)
    {
        var perms = await db.Permission.ToListAsync();
        return Results.Ok(perms);
    }

    public static async Task<IResult> GetAllRolePermissions(AppDbContext db)
    {
        var role_perms = await db.RolePermissions.ToListAsync();
        return Results.Ok(role_perms);
    }
}