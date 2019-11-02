using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            using (var context = new SoftUniContext())
            {
                var result = RemoveTown(context);

                Console.WriteLine(result);


            }
        }

        //3. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();


            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //4. Employees with salary over 50 0000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {

            var result = context
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            return string.Join(Environment.NewLine, result.Select(e => $"{e.FirstName} - {e.Salary:f2}"));

        }

        //5. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {

            var result = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();


            return string.Join(Environment.NewLine, result.Select(e => $"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:f2}"));
        }

        //6. Adding a new address and updating Employee

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15"
                ,
                TownId = 4
            };
            context.Addresses.Add(address);
            context.SaveChanges();

            var emp = context.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            emp.AddressId = address.AddressId;

            context.SaveChanges();

            var texts = context.Employees
                .OrderByDescending(e => e.Address.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            return string.Join(Environment.NewLine, texts);
        }

        //7. Employees And Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.EmployeesProjects
                .Any(ep => 2001 <= ep.Project.StartDate.Year && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    managerFirst = e.Manager.FirstName,
                    managerLast = e.Manager.LastName,
                    projects = e.EmployeesProjects.Select(ep => new
                    {
                        Name = ep.Project.Name,
                        StartDate = ep.Project.StartDate
                        ,
                        EndDate = ep.Project.EndDate

                    }).ToList()
                })
                .ToList();


            foreach (var em in employees)
            {
                sb.AppendLine($"{em.FirstName} {em.LastName} - Manager: {em.managerFirst} {em.managerLast}");

                foreach (var p in em.projects)
                {
                    string projectName = p.Name;
                    string startDate = p.StartDate.ToString(@"M/d/yyyy h:mm:ss tt").Replace('-', '/');

                    string endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString(@"M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).Replace('-', '/') : "not finished";

                    sb.AppendLine($"--{p.Name} - {startDate} - {endDate}");
                }

            }




            return sb.ToString().TrimEnd();
        }

        //8. Address By Town
        public static string GetAddressesByTown(SoftUniContext context)
        {

            var addresses = context
                .Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    a.Town.Name,
                    a.Employees.Count

                })
                .ToList();



            return string.Join(Environment.NewLine, addresses.Select(a => $"{a.AddressText}, {a.Name} - {a.Count} employees"));

        }

        //9. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var emp = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle
                    ,
                    projects = e.EmployeesProjects.Select(ep => new
                    {
                        name = ep.Project.Name
                    })
                    .OrderBy(n => n.name)
                    .ToList()
                })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");

            foreach (var p in emp.projects)
            {
                sb.AppendLine(p.name);
            }

            return sb.ToString().TrimEnd();

        }

        //10. Departments with more than 5 employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name
                    ,
                    d.Manager.FirstName
                    ,
                    d.Manager.LastName
                    ,
                    employees = d.Employees
                    .Select(e => new
                    {
                        e.FirstName
                        ,
                        e.LastName
                        ,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()

                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.FirstName} {d.LastName}");

                foreach (var e in d.employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //11. Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {

            var projects =
                context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name
                    ,
                    p.Description
                    ,
                    p.StartDate
                })
                .OrderBy(p => p.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }


            return sb.ToString().TrimEnd();

        }

        //12. Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {

            context
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                //if you use select, it will create a new object, do not use select!!! 
                //foreach extended body uses readonly data! 
                //.Select(e => e.Salary)
                .ToList()
               .ForEach(e => e.Salary *= 1.12m);

            context.SaveChanges();

            var result = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.Salary
               })
               .OrderBy(e => e.FirstName)
               .ThenBy(e => e.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var e in result)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }
            return sb.ToString().TrimEnd();

        }

        //13 Find Employees by FIrst Name starting with "sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Employees
                .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    first = e.FirstName,
                    last = e.LastName,
                    job = e.JobTitle,
                    salary = e.Salary
                })
                .ToList()
                .ForEach(e => sb.AppendLine($"{e.first} {e.last} - {e.job} - (${e.salary:f2})"));


            return sb.ToString().TrimEnd();

        }

        //14 Delete project by id 
        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects
              .FirstOrDefault(p => p.ProjectId == 2);

            var projects = context.EmployeesProjects
                .Where(p => p.ProjectId == 2);

            context.RemoveRange(projects);

            context.Projects.Remove(project);

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var p in context.Projects.Take(10))
            {
                sb.AppendLine(p.Name);
            }


            return sb.ToString().TrimEnd();
        }

        //15 Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            string name = "Seattle";

            var towns = context.Towns.Where(t => t.Name == name).ToList();

            var addresses = context.Addresses.Where(a => a.Town.Name == name).ToList();

            var count = addresses.Count;

            var addressesNames = addresses.Select(a => a.AddressText);

            context.
                Employees
                .Where(e => addressesNames.Contains(e.Address.AddressText))
                .ToList()
                .ForEach(i => i.AddressId = null);

            context.Addresses.RemoveRange(addresses);
            context.Towns.RemoveRange(towns);

            context.SaveChanges();


            return $"{count} addresses in Seattle were deleted";
        }

    }
}
