using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class FacultyDtoRead
{
    public FacultyDtoRead()
    {
        Groups = new List<string>();
    }

    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public int OrganizationId { get; set; }
    public string Organization { get; set; }
    public ICollection<string> Groups { get; set; }
}