using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class FacultyDtoWrite
{
    public FacultyDtoWrite()
    {
        GroupsIds = new List<int>();
    }
    [Required] public string Name { get; set; }
    [Required] public int OrganizationId { get; set; }
    public ICollection<int>? GroupsIds { get; set; }
}