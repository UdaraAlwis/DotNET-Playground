using System;

Console.WriteLine("Hello World from .NET 5 and C# 9!");
int value1 = 1;
int value2 = 2;
Console.WriteLine($"Adding Values {value1}+{value2}={AddTwoValues(value1, value2)}");

static int AddTwoValues(int value1, int value2)
{
    return value1 + value2;
}
