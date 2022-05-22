using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class TimeTableDtoRead
{
    
    public int Id { get; set; }
    [Required] public int LectorId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int GroupId { get; set; }
    [Required] public int Auditory { get; set; }
    [Required] public int Lection { get; set; }
    [Required] public string Day { get; set; }

    public string Group { get; set; }
    public string Subject { get; set; }
    public string Lector { get; set; }
}