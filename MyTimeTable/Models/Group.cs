using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.Models;

public class Group
{
    public Group()
    {
        TimeTables = new List<TimeTable>();
    }

    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public int Course { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public int FacultyId { get; set; }

    public Faculty Faculty { get; set; }
    public ICollection<TimeTable> TimeTables { get; set; }
}