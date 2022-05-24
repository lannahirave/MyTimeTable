namespace MyTimeTable.ModelsDTO;

public class GroupDtoWrite
{
    public GroupDtoWrite()
    {
        TimetablesIds = new List<int>();
    }
    public string Name { get; set; }
    public int Course { get; set; }
    public int Quantity { get; set; }
    public int FacultyId { get; set; }
    
    public ICollection<int>? TimetablesIds { get; set; }
}