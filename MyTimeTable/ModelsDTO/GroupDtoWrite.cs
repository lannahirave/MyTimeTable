using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class GroupDtoWrite
{
    public GroupDtoWrite()
    {
        TimetablesIds = new List<int>();
    }
    [Required] public string Name { get; set; }
    [Required] public int Course { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public int FacultyId { get; set; }
    
    public ICollection<int>? TimetablesIds { get; set; }
}