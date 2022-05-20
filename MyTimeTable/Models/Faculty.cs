using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.Models
{
    public class Faculty
    {
        public Faculty()
        {
            Groups = new List<Group>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        
        public Organization Organization { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
