using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.Models;

public class TimeTable
{
    public int Id { get; set; }
    [Required] public int LectorId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int GroupId { get; set; }
    [Required] public int Auditory { get; set; }
    [Required] public int Lection { get; set; }
    [Required] public string Day { get; set; }

    public Group Group { get; set; }
    public Subject Subject { get; set; }
    public Lector Lector { get; set; }
}