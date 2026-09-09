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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasIndex(course => course.Code)
            .IsUnique();
    }
}