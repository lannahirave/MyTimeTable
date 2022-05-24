using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class LectorDtoRead
{
    public LectorDtoRead()
    {
        OrganizationsIds = new List<int>();
        Organizations = new List<string>();
    }
    public int Id { get; set; }
    public string FullName { get; set; }
    [DataType(DataType.PhoneNumber, ErrorMessage = "Має бути номером телефону.")]
    public int Phone { get; set; }
    public string Degree { get; set; }
    public ICollection<int> OrganizationsIds { get; set; }
    public ICollection<string> Organizations { get; set; }
}