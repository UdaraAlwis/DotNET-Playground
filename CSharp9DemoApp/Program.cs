using CSharp9DemoApp;
using System;
using System.Linq;

Console.WriteLine("Hello World from .NET 5 and C# 9!");
int value1 = 1;
int value2 = 2;
Console.WriteLine($"Adding Values {value1}+{value2}={AddTwoValues(value1, value2)}");

//var person = new PersonModel() { Id = 1, FirstName = "Bim", LastName = "Pim" };
//PersonModel person = new(1, "Bim", "Pim") { };
PersonModel person = new () { Id = 1, FirstName = "Bim", LastName = "Pim" };

//// DOES NOT WORK - init setter
//person.Id = 2;

// Pattern matching - not null
if (person is not null)
{
    Console.WriteLine($"Id: {person.Id} First Name: {person.FirstName} Last Name: {person.LastName}");
}

var rng = new Random();
foreach (var age in Enumerable.Range(0, 5).Select(index => index = rng.Next(-1,19)))
{
    Console.WriteLine($"Age: {age} - {GetDescriptionForAge(age)}");
}

static int AddTwoValues(int value1, int value2)
{
    return value1 + value2;
}


// Relational Pattern Matching > < <= >=
// Logical Pattern Matching and, or & not

string GetDescriptionForAge(int age)
{
    return age switch
    {
        <= 0 => "Welcome to the World!",
        > 0 and < 5 => "Time for Pre-School!",
        6 or 7 => "So how's your Primary School?",
        > 7 and < 18 => "Enjoy your Mid-School years!",
        >= 18 => "You an adult! no more handouts, Good luck!",
        _ => "Hmmmm, I'm not sure!",
    };
}