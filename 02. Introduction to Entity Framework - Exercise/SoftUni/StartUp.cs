using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        private static SoftUniContext dbContext = new SoftUniContext();

        private static void Main(string[] args)
        {
            //var task3 = GetEmployeesFullInformation(dbContext);
            //Console.WriteLine(task3);

            //var task4 = GetEmployeesWithSalaryOver50000(dbContext);
            //Console.WriteLine(task4);

            //var task5 = GetEmployeesFromResearchAndDevelopment(dbContext);
            //Console.WriteLine(task5);

            //var task6 = AddNewAddressToEmployee(dbContext);
            //Console.WriteLine(task6);

            //var task7 = GetEmployeesInPeriod(dbContext);
            //Console.WriteLine(task7);

            //var task8 = GetAddressesByTown(dbContext);
            //Console.WriteLine(task8);

            //var task9 = GetEmployee147(dbContext);
            //Console.WriteLine(task9);

            //var task10 = GetDepartmentsWithMoreThan5Employees(dbContext);
            //Console.WriteLine(task10);

            var task11 = GetLatestProjects(dbContext);
            Console.WriteLine(task11);

            //var task12 = IncreaseSalaries(dbContext);
            //Console.WriteLine(task12);

            //var task13 = GetEmployeesByFirstNameStartingWithSa(dbContext);
            //Console.WriteLine(task13);

            //var task14 = DeleteProjectById(dbContext);
            //Console.WriteLine(task14);

            //var task15 = RemoveTown(dbContext);
            //Console.WriteLine(task15);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
                .ToList()
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees) 
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees) 
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(e => e.Departments)
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department} - ${employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            string newAddressText = "Vitoshka 15";
            int newTownId = 4;

            var employeeNakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

            if (employeeNakov != null)
            {
                var newAddress = new Address()
                {
                    AddressText = newAddressText,
                    TownId = newTownId
                };

                employeeNakov.Address = newAddress;

                context.SaveChanges();
            }

            var addresses = context.Employees
                .Include(e => e.Address)
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            var result = new StringBuilder();

            foreach (var address in addresses)
            {
                result.AppendLine(address);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(e => e.Manager)
                .ThenInclude(e => e.Projects)
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.Projects
                    .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                    .Select(p => new
                    {
                        p.Name,
                        p.StartDate,
                        p.EndDate
                    })
                    .ToList()
                })
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    if (project.EndDate != null)
                    {
                        result.AppendLine($"--{project.Name} - {project.StartDate} - {project.EndDate}");
                    }
                    else
                    {
                        result.AppendLine($"--{project.Name} - {project.StartDate} - not finished");
                    }
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Include(a => a.Employees)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var address in addresses)
            {
                result.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee147 = context.Employees
                .Include(e => e.Projects)
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.Projects.OrderBy(p => p.Name).ToList()
                })
                .FirstOrDefault();

            var result = new StringBuilder();

            result.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employee147.Projects)
            {
                result.AppendLine(project.Name);
            }

            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Include(d => d.Employees)
                .ThenInclude(d => d.Manager)
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    EmployeesCount = d.Employees.Count,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()
                })
                .OrderBy(d => d.EmployeesCount)
                .ThenBy(d => d.Name)
                .ToList();

            var result = new StringBuilder();

            foreach (var department in departments)
            {
                result.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            var result = new StringBuilder();

            foreach (var project in projects)
            {
                result.AppendLine(project.Name);
                result.AppendLine(project.Description);
                result.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = new string[4] 
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employees = context.Employees
                .Include(e => e.Departments)
                .Where(e => departments.Contains(e.Department.Name))
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary += employee.Salary * 0.12m;
            }

            context.SaveChanges();

            var finalEmployees = employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in finalEmployees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return result.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeeProjects = context.EmployeesProjects
                .Include(p => p.Project)
                .Where(p => p.ProjectId == 2)
                .ToList();

            context.EmployeesProjects.RemoveRange(employeeProjects);

            var project = context.Projects.FirstOrDefault(p => p.ProjectId == 2);

            if (project != null)
            {
                context.Projects.Remove(project);

                context.SaveChanges();
            }

            var allProjects = context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            var result = new StringBuilder();

            foreach (var currProject in allProjects)
            {
                result.AppendLine(currProject);
            }

            return result.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(e => e.Address)
                .ThenInclude(e => e.Town)
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            context.SaveChanges();

            var addresses = context.Addresses.Include(a => a.Town).Where(t => t.Town.Name == "Seattle").ToList();

            int removedAddresses = addresses.Count;

            context.Addresses.RemoveRange(addresses);

            context.SaveChanges();

            var town = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

            if (town != null)
            {
                context.Towns.Remove(town);

                context.SaveChanges();
            }

            return $"{removedAddresses} addresses in Seattle were deleted";
        }
    }
}