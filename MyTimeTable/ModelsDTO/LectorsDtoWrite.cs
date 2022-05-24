using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.ModelsDTO;

public class LectorsDtoWrite
{
    // FOR POST AND PUT METHODS COS WE CANT EXPLICITLY SET ORGANIZATION SO WE POST THEIRS IDS
    public LectorsDtoWrite()
    {
        OrganizationsIds = new List<int>();
    }
    public string FullName { get; set; }
    [DataType(DataType.PhoneNumber, ErrorMessage = "Має бути номером телефону.")]
    public int Phone { get; set; }
    public string Degree { get; set; }
    public ICollection<int>? OrganizationsIds { get; set; }

}