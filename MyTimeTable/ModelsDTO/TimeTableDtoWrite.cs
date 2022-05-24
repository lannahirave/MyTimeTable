namespace MyTimeTable.ModelsDTO;

public class TimeTableDtoWrite
{
    public int LectorId { get; set; }
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
    public int Auditory { get; set; }
    public int Lection { get; set; }
    public string Day { get; set; }
}