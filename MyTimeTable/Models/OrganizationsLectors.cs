namespace MyTimeTable.Models;

public class OrganizationsLectors
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public int LectorId { get; set; }

    public Lector Lector { get; set; }
    public Organization Organization { get; set; }
}