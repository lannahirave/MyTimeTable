using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.Models;

public class Lector
{
    public Lector()
    {
        Organizations = new List<Organization>();
        TimeTables = new List<TimeTable>();
    }

    public int Id { get; set; }
    [Required] public string FullName { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Має бути номером телефону.")]
    public int Phone { get; set; }

    [Required] public string Degree { get; set; }
    [Required] public int OrganizationId { get; set; }
    public ICollection<Organization> Organizations { get; set; }
    public ICollection<TimeTable> TimeTables { get; set; }
}