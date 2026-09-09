using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Api.Data;
using StudentEnrollment.Api.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/students", async (AppDbContext db) =>
{
    var students = await db.Students.ToListAsync();

    return Results.Ok(students);
});

app.MapPost("/api/students", async (Student newStudent, AppDbContext db) =>
{
    db.Students.Add(newStudent);

    await db.SaveChangesAsync();

    return Results.Created($"/api/students/{newStudent.Id}", newStudent);
});

app.MapGet("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FindAsync(id);

    if (student is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(student);
});

app.MapPut("/api/students/{id}", async (int id, Student updatedStudent, AppDbContext db) =>
{
    var student = await db.Students.FindAsync(id);

    if (student is null)
    {
        return Results.NotFound();
    }

    student.Name = updatedStudent.Name;
    student.Age = updatedStudent.Age;
    student.Course = updatedStudent.Course;

    await db.SaveChangesAsync();

    return Results.Ok(student);
});

app.MapDelete("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FindAsync(id);

    if (student is null)
    {
        return Results.NotFound();
    }

    db.Students.Remove(student);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/api/courses", async (AppDbContext db) =>
{
    var courses = await db.Courses
        .OrderBy(course => course.Name)
        .ToListAsync();

    return Results.Ok(courses);
});

app.MapGet("/api/courses/{id}", async (int id, AppDbContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(course);
});

app.MapPost("/api/courses", async (Course newCourse, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(newCourse.Code) ||
        string.IsNullOrWhiteSpace(newCourse.Name))
    {
        return Results.BadRequest("Course code and name are required.");
    }

    if (newCourse.Fee < 0)
    {
        return Results.BadRequest("Course fee cannot be negative.");
    }

    newCourse.Code = newCourse.Code.Trim().ToUpper();
    newCourse.Name = newCourse.Name.Trim();

    var codeExists = await db.Courses
        .AnyAsync(course => course.Code == newCourse.Code);

    if (codeExists)
    {
        return Results.Conflict("Course code already exists.");
    }

    db.Courses.Add(newCourse);
    await db.SaveChangesAsync();

    return Results.Created($"/api/courses/{newCourse.Id}", newCourse);
});

app.MapPut("/api/courses/{id}", async (
    int id,
    Course updatedCourse,
    AppDbContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(updatedCourse.Code) ||
        string.IsNullOrWhiteSpace(updatedCourse.Name))
    {
        return Results.BadRequest("Course code and name are required.");
    }

    var normalizedCode = updatedCourse.Code.Trim().ToUpper();

    var codeExists = await db.Courses.AnyAsync(existingCourse =>
        existingCourse.Code == normalizedCode &&
        existingCourse.Id != id);

    if (codeExists)
    {
        return Results.Conflict("Course code already exists.");
    }

    course.Code = normalizedCode;
    course.Name = updatedCourse.Name.Trim();
    course.Fee = updatedCourse.Fee;
    course.IsActive = updatedCourse.IsActive;

    await db.SaveChangesAsync();

    return Results.Ok(course);
});

app.MapDelete("/api/courses/{id}", async (int id, AppDbContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course is null)
    {
        return Results.NotFound();
    }

    db.Courses.Remove(course);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();