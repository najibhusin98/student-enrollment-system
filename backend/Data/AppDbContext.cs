using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Api.Models;

namespace StudentEnrollment.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<Course> Courses { get; set; } = null!;

    public DbSet<Enrollment> Enrollments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasIndex(course => course.Code)
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasIndex(enrollment => new
            {
                enrollment.StudentId,
                enrollment.CourseId
            })
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasOne(enrollment => enrollment.Student)
            .WithMany(student => student.Enrollments)
            .HasForeignKey(enrollment => enrollment.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(enrollment => enrollment.Course)
            .WithMany(course => course.Enrollments)
            .HasForeignKey(enrollment => enrollment.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}