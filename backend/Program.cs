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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

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

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
