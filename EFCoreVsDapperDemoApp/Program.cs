using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bogus;
using Dapper;
using EFCoreVsDapperDemoApp.Models;

namespace EFCoreVsDapperDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Welcome to a little playground with EntityFramework vs Dapper!");

            // Dapper Stuff
            Console.WriteLine(">> Reading data with Dapper");
            List<Employee> employees = GetEmployees_Dapper();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee ID: {employee.EmployeeId} | Employee Name: {employee.FirstName} {employee.LastName}");
            }

            Console.WriteLine(">> Writing data with Dapper");
            var employeeFaker = new Faker<Employee>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName);
            var employeeFakeData = employeeFaker.Generate();
            var result = WriteEmployee_Dapper(employeeFakeData);
            Console.WriteLine($"New Employee Id: {result}");

            Console.WriteLine(">> Reading written data with Dapper");
            var resultEmployee = GetEmployeeById_Dapper(result).FirstOrDefault();
            Console.WriteLine($"Employee ID: {resultEmployee.EmployeeId} | Employee Name: {resultEmployee.FirstName} {resultEmployee.LastName}");





        }

        private static int WriteEmployee_Dapper(Employee employee)
        {
            string sql = "INSERT INTO Employees (FirstName, LastName) Values (@FirstName, @LastName)";

            IDbConnection conn = new SqlConnection(Connections.DbConnection.GetConnectionString());
            var writeResult = conn.Execute(sql,
                new
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                });

            var newEmployeeId = 0;
            if (writeResult == 1)
            {
                sql = "SELECT TOP 1 * FROM Employees ORDER BY EmployeeID DESC";
                var readResults = conn.Query<Employee>(sql).AsList<Employee>();
                newEmployeeId = readResults.First().EmployeeId;
            }

            conn.Close();
            return newEmployeeId;
        }

        private static List<Employee> GetEmployees_Dapper()
        {
            string sql = "SELECT * FROM Employees";

            IDbConnection conn = new SqlConnection(Connections.DbConnection.GetConnectionString());
            var results = conn.Query<Employee>(sql).AsList<Employee>();
            conn.Close();

            return results;
        }

        private static List<Employee> GetEmployeeById_Dapper(int EmployeeId)
        {
            string sql = $"SELECT * FROM Employees WHERE EmployeeID='{EmployeeId}'";

            IDbConnection conn = new SqlConnection(Connections.DbConnection.GetConnectionString());
            var results = conn.Query<Employee>(sql).AsList<Employee>();
            conn.Close();

            return results;
        }

    }
}
