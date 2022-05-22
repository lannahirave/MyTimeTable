using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class LectorsDtoWrite
{
    public LectorsDtoWrite()
    {
        OrganizationsIds = new List<int>();
    }
    [Required] public string FullName { get; set; }
    [Required]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Має бути номером телефону.")]
    public int Phone { get; set; }
    [Required] public string Degree { get; set; }
    public ICollection<int>? OrganizationsIds { get; set; }

}