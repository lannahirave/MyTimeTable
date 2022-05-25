using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class OrganizationDtoWrite
{
    public OrganizationDtoWrite()
    {
        FacultiesIds = new List<int>();
    }
    [Required]public string Name { get; set; }
    public ICollection<int>? FacultiesIds { get; set; }
}