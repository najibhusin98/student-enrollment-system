namespace StudentEnrollment.Api.Models;

public class Course
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Fee { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();
}