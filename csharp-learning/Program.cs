Console.WriteLine("=== Student Enrollment System ===");

string studentName = GetValidStudentName();
int age = GetValidAge();
string courseName = GetValidCourseName();
decimal courseFee = GetValidFee();

bool isAdult = age >= 18;

Console.WriteLine();
Console.WriteLine("=== Student Details ===");
Console.WriteLine($"Name: {studentName}");
Console.WriteLine($"Age: {age}");
Console.WriteLine($"Course: {courseName}");
Console.WriteLine($"Course Fee: RM{courseFee:F2}");

if (isAdult)
{
    Console.WriteLine("Status: Eligible for enrollment");
}
else
{
    Console.WriteLine("Status: Requires guardian approval");
}


static string GetValidStudentName()
{
    while (true)
    {
        Console.Write("Enter student name: ");
        string? studentName = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(studentName) &&
            studentName.Any(char.IsLetter))
        {
            return studentName;
        }

        Console.WriteLine("Invalid student name. Please enter a valid name.");
    }
}


static int GetValidAge()
{
    while (true)
    {
        Console.Write("Enter student age: ");
        string? ageInput = Console.ReadLine();

        if (int.TryParse(ageInput, out int age) &&
            age > 0 &&
            age <= 120)
        {
            return age;
        }

        Console.WriteLine("Invalid age. Please enter an age between 1 and 120.");
    }
}


static string GetValidCourseName()
{
    while (true)
    {
        Console.Write("Enter course name: ");
        string? courseName = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(courseName) &&
            courseName.Any(char.IsLetter))
        {
            return courseName;
        }

        Console.WriteLine("Invalid course name. Please enter a valid course name.");
    }
}


static decimal GetValidFee()
{
    while (true)
    {
        Console.Write("Enter course fee: RM");
        string? feeInput = Console.ReadLine();

        if (decimal.TryParse(feeInput, out decimal fee) &&
            fee > 0)
        {
            return fee;
        }

        Console.WriteLine("Invalid course fee. Please enter an amount greater than 0.");
    }
}