namespace MyTimeTable.ModelsDTO;

public class GroupDtoRead
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Course { get; set; }
    public int Quantity { get; set; }
    public int FacultyId { get; set; }

    public string? Faculty { get; set; }
}