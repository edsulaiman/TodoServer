namespace TodoServer.Dtos;

public class OutTodoDto
{
    public long? Id { get; set; }
    public long? ParentId { get; set; }
    public string? TaskName { get; set; }
    public bool? IsDone { get; set; }
    public long? StartDate { get; set; }
    public long? DueDate { get; set; }
    public bool? Starred { get; set; }
    public long? CreatedAt { get; set; }
    public long? UpdatedAt { get; set; }
}