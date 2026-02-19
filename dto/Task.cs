public class PostTaskDto
{
    public required string title { get; set;}
    public required string description { get; set;}
    public int status_id { get; set;}
}

public class UpdateTaskDto
{
    public string? title { get; set;}
    public string? description { get; set;}
}