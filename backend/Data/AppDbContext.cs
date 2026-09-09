using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Api.Models;

namespace StudentEnrollment.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}