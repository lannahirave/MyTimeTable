namespace MyTimeTable.Models;

public class Control
{
    public Control()
    {
        Subjects = new List<Subject>();
    }

    public int Id { get; set; }

    public string Type { get; set; }

    public ICollection<Subject> Subjects { get; set; }
}