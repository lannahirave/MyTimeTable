using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class OrganizationDtoWrite
{
    public OrganizationDtoWrite()
    {
        Faculties = new List<string>();
        FacultiesIds = new List<int>();
    }

    public int Id { get; set; }
    [Required] public string Name { get; set; }
    public ICollection<string>? Faculties { get; set; }
    public ICollection<int>? FacultiesIds { get; set; }
}