using Microsoft.EntityFrameworkCore;
using tasksApi;

public static class Rbac
{
    public static void MapRbacRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/rbac");

        //roles
        group.MapGet("/roles/", GetAllRoles);
        group.MapPost("/roles/", PostRole);
        group.MapPatch("/roles/{id}", UpdateRole);

        //permissions
        group.MapGet("/permissions/", GetAllPermissions);
        group.MapPost("/permissions/", PostPermission);
        group.MapPatch("/permissions/{id}", UpdatePermission);

        //role_permissions
        group.MapGet("/role_permissions/", GetAllRolePermissions);
        group.MapPost("/role_permissions/", PostRolePermission);
    }

    //Roles endpoints
    public static async Task<IResult> GetAllRoles(AppDbContext db)
    {
        var roles = await db.Role.ToListAsync();
        return Results.Ok(roles);
    }

    public static async Task<IResult> PostRole(string name, AppDbContext db)
    {
        var exists = await db.Role.SingleOrDefaultAsync(u => u.name == name);
        if(exists != null)
        {
             return Results.Problem($"Role with {name} already exists");
        }

        var role = new Role
        {
            name = name
        };

        db.Role.Add(role);
        await db.SaveChangesAsync();
        return Results.Created($"/api/rbac/roles/{role.id}", role);
    }

    public static async Task<IResult> UpdateRole(int id, string name, AppDbContext db)
    {
        var role = await db.Role.FindAsync(id);
        if (role == null)
        {
            return Results.NotFound($"Role width id ${id} does not exists");
        }

        role.name = name;

        try
        {
            await db.SaveChangesAsync();
            return Results.Ok($"Role {id} updated");
        } 
            catch (Exception ex)
        {
            return Results.Problem($"Unable to update: {ex}");
        }
    }

    //Permissions endpoints
    public static async Task<IResult> GetAllPermissions(AppDbContext db)
    {
        var perms = await db.Permission.ToListAsync();
        return Results.Ok(perms);
    }

    public static async Task<IResult>PostPermission(PostPermission payload, AppDbContext db)
    {
        var exists = await db.Permission.FirstOrDefaultAsync(u => u.codename == payload.codename);

        if (exists != null)
        {
            return Results.Problem($"Permission with {payload.codename} already exists");
        }

        var perm = new Permission
        {
            name = payload.name,
            codename = payload.codename
        };

        db.Permission.Add(perm);
        await db.SaveChangesAsync();
        return Results.Created($"/api/rbac/permission/{perm.id}", perm);
    }

    public static async Task<IResult>UpdatePermission(int id, PostPermission payload, AppDbContext db)
    {
        var perm = await db.Permission.FindAsync(id);

        if(perm == null)
        {
            return Results.NotFound($"Permission with id {id} not found");
        }

        perm.name = payload.name;
        perm.codename = payload.codename;

        try
        {
            await db.SaveChangesAsync();
            return Results.Ok($"Permission {id} updated");
        } 
            catch (Exception ex)
        {
            return Results.Problem($"Unable to update: {ex}");
        }
    }

    //RolePermissions endpoints
    public static async Task<IResult> GetAllRolePermissions(AppDbContext db)
    {
        var rolePermissions = await db.RolePermissions
            .Include(rp => rp.role)
            .Include(rp => rp.permission)
            .ToListAsync();

        // Group by role
        var result = rolePermissions
            .GroupBy(rp => new { rp.role_id, RoleName = rp.role != null ? rp.role.name : "Unknown" })
            .Select(g => new RoleWithPermissionsDto
            {
                role_id = g.Key.role_id,
                role_name = g.Key.RoleName,
                permissions = g.Select(rp => new PermissionInfoDto
                {
                    permission_id = rp.permission_id,
                    permission_name = rp.permission != null ? rp.permission.name : "Unknown",
                    permission_codename = rp.permission != null ? rp.permission.codename : "Unknown"
                }).ToList()
            })
            .ToList();

        return Results.Ok(result);
    }

    public static async Task<IResult> PostRolePermission(CreateRolePermission payload, AppDbContext db)
    {
        var role = await db.Role.FindAsync(payload.role_id);
        if (role == null)
        {
            return Results.Problem($"Role with ID {payload.role_id} does not exist");
        }

        var permission = await db.Permission.FindAsync(payload.permission_id);
        if (permission == null)
        {
            return Results.Problem($"Permission with ID {payload.permission_id} does not exist");
        }

        var exists = await db.RolePermissions
            .FirstOrDefaultAsync(rp => rp.role_id == payload.role_id && rp.permission_id == payload.permission_id);
        
        if (exists != null)
        {
            return Results.Problem($"Role '{role.name}' already has permission '{permission.name}'");
        }

        var rolePermission = new RolePermissions
        {
            role_id = payload.role_id,
            permission_id = payload.permission_id
        };

        await db.RolePermissions.AddAsync(rolePermission);
        await db.SaveChangesAsync();

        var response = new RolePermissionResponse
        {
            id = rolePermission.id,
            role_id = rolePermission.role_id,
            permission_id = rolePermission.permission_id,
            role_name = role.name,
            permission_name = permission.name,
            permission_codename = permission.codename
        };

        return Results.Created($"/api/rolepermissions/{rolePermission.id}", response);
    }
}