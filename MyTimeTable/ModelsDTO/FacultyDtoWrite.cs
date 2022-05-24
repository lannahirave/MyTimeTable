namespace MyTimeTable.ModelsDTO;

public class FacultyDtoWrite
{
    public FacultyDtoWrite()
    {
        GroupsIds = new List<int>();
    }
    public string Name { get; set; }
    public int OrganizationId { get; set; }
    public ICollection<int>? GroupsIds { get; set; }
}