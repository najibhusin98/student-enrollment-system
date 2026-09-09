namespace StudentEnrollment.Api.Models;

public class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();

}