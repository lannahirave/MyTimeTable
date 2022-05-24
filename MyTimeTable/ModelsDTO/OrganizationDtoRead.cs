namespace MyTimeTable.ModelsDTO;

public class OrganizationDtoRead
{
    public OrganizationDtoRead()
    {
        Faculties = new List<string>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<string>? Faculties { get; set; }
}