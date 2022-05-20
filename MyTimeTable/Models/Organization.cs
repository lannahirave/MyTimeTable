using System.ComponentModel.DataAnnotations;

namespace MyTimeTable.Models
{
    public class Organization
    {
        public Organization()
        {
            Faculties = new List<Faculty>();
            Lectors = new List<Lector>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Faculty> Faculties { get; set; }
        public ICollection<Lector> Lectors { get; set; }
    }
}
