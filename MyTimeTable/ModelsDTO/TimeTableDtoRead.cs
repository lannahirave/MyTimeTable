namespace MyTimeTable.ModelsDTO;

public class TimeTableDtoRead
{
    
    public int Id { get; set; }
    public int LectorId { get; set; }
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
    public int Auditory { get; set; }
    public int Lection { get; set; }
    public string Day { get; set; }

    public string Group { get; set; }
    public string Subject { get; set; }
    public string Lector { get; set; }
}