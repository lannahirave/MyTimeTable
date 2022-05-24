namespace MyTimeTable.ModelsDTO;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Hours { get; set; }
    public int ControlId { get; set; }
    public string? ControlType { get; set; }
}