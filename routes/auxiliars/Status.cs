using Microsoft.EntityFrameworkCore;

public static class Status
{
    public static void MapStatusRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/auxiliars/status").RequireAuthorization().WithOpenApi();
        group.MapGet("/", GetAllStatus).RequireAuthorization().WithOpenApi();
        group.MapGet("/{id}", GetStatus).RequireAuthorization().WithOpenApi();
        group.MapPost("/", PostStatus).RequireAuthorization().WithOpenApi();
    }

    public static async Task<IResult> GetAllStatus(AppDbContext db)
    {
        var status = await db.Status.ToListAsync();
        return Results.Ok(status);
    }

    public static async Task<IResult> GetStatus(int id, AppDbContext db)
    {
        var status = await db.Status.FindAsync(id);
        return Results.Ok(status);
    }

    public static async Task<IResult> PostStatus(StatusModel status, AppDbContext db)
    {
        db.Status.Add(status);
        await db.SaveChangesAsync();
        return Results.Created($"/api/tasks/{status.id}", status);
    }
}