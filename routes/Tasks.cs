using Microsoft.EntityFrameworkCore;

public static class Tasks
{
    public static void MapTasksRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks");
        
        group.MapGet("/", GetAllTasks).RequireAuthorization().WithOpenApi();
        group.MapGet("/{id}", GetTask).RequireAuthorization().WithOpenApi();
        group.MapPost("/", PostTask).RequireAuthorization().WithOpenApi();
        group.MapPatch("/{id}", UpdateTask).RequireAuthorization().WithOpenApi();
        group.MapPut("/{id}", UpdateStatusTask).RequireAuthorization().WithOpenApi();
    }

    public static async Task<IResult> GetAllTasks(AppDbContext db)
    {
        var tasks = await db.Tasks.ToListAsync();
        return Results.Ok(tasks);
    }

    public static async Task<IResult> GetTask(int id, AppDbContext db)
    {
        var task = await db.Tasks.FindAsync(id);
        return Results.Ok(task);
    }

    public static async Task<IResult> PostTask(PostTaskDto payload, AppDbContext db)
    {
        var exists = await db.Tasks.SingleOrDefaultAsync(u => u.title == payload.title);
        if(exists != null)
        {
            return Results.Problem($"Task with {payload.title} already exists");
        }

        var task = new TaskModel
        {
            title = payload.title,
            description = payload.description,
            status_id = payload.status_id
        };

        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return Results.Created($"/api/tasks/{task.id}", task);
    }

    public static async Task<IResult> UpdateTask(int id, UpdateTaskDto payload, AppDbContext db)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task == null)
        {
            return Results.NotFound($"Task with id {id} not found");
        }

        task.title = payload.title;
        task.description = payload.description;

        try
        {
            await db.SaveChangesAsync();
            return Results.Ok($"Task {id} updated");
        } 
            catch (Exception ex)
        {
            return Results.Problem($"Unable to update: {ex}");
        }
    }

    public static async Task<IResult> UpdateStatusTask(int id, int status_id, AppDbContext db)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task == null)
        {
            return Results.NotFound($"Task with id {id} not found");
        }

        task.status_id = status_id;

        try
        {
            await db.SaveChangesAsync();
            return Results.Ok($"Status of task {id} updated");
        } 
            catch (Exception ex)
        {
            return Results.Problem($"Unable to update status: {ex}");
        }
    }
} 