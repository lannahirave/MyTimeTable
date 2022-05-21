using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class GroupDtoRead
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public int Course { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public int FacultyId { get; set; }

    public string? Faculty { get; set; }
}