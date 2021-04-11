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

            var employeeFaker = new Faker<Employee>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(p => p.City, f => f.Person.Address.City)
                .RuleFor(p => p.Address, f => f.Address.FullAddress());

            // Dapper Stuff
            Console.WriteLine(">> Reading data with Dapper");
            List<Employee> employees = GetEmployees_Dapper();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee ID: {employee.EmployeeId} | Employee Name: {employee.FirstName} {employee.LastName}");
            }

            Console.WriteLine(">> Writing data with Dapper");
            var employeeFakeData = employeeFaker.Generate();
            var result = WriteEmployee_Dapper(employeeFakeData);
            Console.WriteLine($"New Employee Id: {result}");

            Console.WriteLine(">> Reading written data with Dapper");
            var resultEmployee = GetEmployeeById_Dapper(result).FirstOrDefault();
            Console.WriteLine($"Employee ID: {resultEmployee.EmployeeId} | Employee Name: {resultEmployee.FirstName} {resultEmployee.LastName}");


            // EF Stuff
            Console.WriteLine(">> Writing data with EF");
            employeeFakeData = employeeFaker.Generate();
            result = WriteEmployee_EntityFramework(employeeFakeData);
            Console.WriteLine($"Employee Write Result: {result}");

            Console.WriteLine(">> Reading data with EF");
            employees = GetEmployees_EntityFramework();
            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee ID: {employee.EmployeeId} | Employee Name: {employee.FirstName} {employee.LastName}");
            }

            Console.WriteLine(">> Reading Lazy loaded data with EF");
            var employeeWithTerrortoriesResult = GetEmployeesLazily_EntityFramework(1);
            foreach (var employeeWithTerrortories in employeeWithTerrortoriesResult)
            {
                Console.WriteLine($"Employee: {employeeWithTerrortories.Item1.FirstName} {employeeWithTerrortories.Item1.LastName} | " +
                    $"Terrortory: {employeeWithTerrortories.Item2.TerritoryId}");
            }
        }

        // DAPPER BITS
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

        // ENTITY FRAMEWORK BITS
        private static int WriteEmployee_EntityFramework(Employee employee)
        {
            NorthWindContext northWindContext = new NorthWindContext();
            northWindContext.Employees.Add(employee);
            var results = northWindContext.SaveChanges();

            return results;
        }

        private static List<Employee> GetEmployees_EntityFramework()
        {
            NorthWindContext northWindContext = new NorthWindContext();
            return northWindContext.Employees.AsList<Employee>();
        }

        private static List<Tuple<Employee, EmployeeTerritory>> GetEmployeesLazily_EntityFramework(int employeeId)
        {
            NorthWindContext northWindContext = new NorthWindContext();

            // Advantage over Dapper: no need custom models to handle JOIN queries
            var myEmployees = from employee in northWindContext.Set<Employee>().Where(e => e.EmployeeId == employeeId)
                              join employeeterrotory in northWindContext.Set<EmployeeTerritory>()
                              on employee.EmployeeId equals employeeterrotory.EmployeeId
                              select new { employee, employeeterrotory };
            // At this point myEmployees quesry is Lazy loaded

            // So next, let's actually pull the data
            var queryResult = myEmployees.ToList();

            var finalResult = new List<Tuple<Employee, EmployeeTerritory>>();
            foreach (var item in queryResult)
            {
                var tupledResult = Tuple.Create(item.employee, item.employeeterrotory);
                finalResult.Add(tupledResult);
            }
            return finalResult;
        }
    }
}
