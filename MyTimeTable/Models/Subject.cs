using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTimeTable.Models;

public class Subject
{
    public Subject()
    {
        TimeTables = new List<TimeTable>();
    }

    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Type { get; set; }
    [Required] public int Hours { get; set; }
    [Required] public int ControlId { get; set; }
    [Display(Name = "Тип контролю")] public Control Control { get; set; }
    public ICollection<TimeTable> TimeTables { get; set; }
}