using System.ComponentModel.DataAnnotations.Schema;

public class TaskModel
{
    public int id {get; set;}
    public string title { get; set;}
    public string description { get; set;}
    [ForeignKey("StatusModel")]
    public int status_id {get; set;}

    public virtual StatusModel? status { get; set;}
    public bool is_done { get; set; } = false;
    public DateTime created_at { get; set;} = DateTime.UtcNow;

    public TaskModel() {
        status_id = 1;
    }
}