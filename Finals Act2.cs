using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }                  // Column 1
    public string FirstName { get; set; } = "";  // Column 2
    public string LastName { get; set; } = "";   // Column 3
    public string Department { get; set; } = ""; // Column 4
    public string JobTitle { get; set; } = "";   // Column 5
    public decimal MonthlySalary { get; set; }   // Column 6
    public double TaxRate { get; set; }          // Column 7
    public DateTime HireDate { get; set; }       // Column 8
    public bool IsActive { get; set; }           // Column 9
    public string Email { get; set; } = "";      // Column 10
}

class Program
{
    // Starts EMPTY (0 pre-defined records)
    static List<Employee> employees = new List<Employee>();
    static int nextId = 1001;

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE PAYROLL & HR MANAGEMENT SYSTEM ===");
            Console.WriteLine("1. Create (Add Employee)");
            Console.WriteLine("2. Read (View All Employees)");
            Console.WriteLine("3. Search / Filter Employees");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": CreateEmployee(); break;
                case "2": ReadEmployees(); break;
                case "3": SearchEmployees(); break;
                case "4": UpdateEmployee(); break;
                case "5": DeleteEmployee(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // ---------- CREATE ----------
    static void CreateEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");

        Console.Write("First Name: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Last Name: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Department (e.g., IT, HR, Sales, Finance): ");
        string department = Console.ReadLine() ?? "";

        Console.Write("Job Title: ");
        string jobTitle = Console.ReadLine() ?? "";

        decimal salary;
        Console.Write("Monthly Salary: ");
        while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0)
            Console.Write("Invalid amount. Enter Monthly Salary: ");

        double taxRate;
        Console.Write("Tax Rate (e.g., 0.12 for 12%): ");
        while (!double.TryParse(Console.ReadLine(), out taxRate) || taxRate < 0 || taxRate > 1)
            Console.Write("Invalid rate (0 to 1). Enter Tax Rate: ");

        DateTime hireDate;
        Console.Write("Hire Date (yyyy-mm-dd): ");
        while (!DateTime.TryParse(Console.ReadLine(), out hireDate))
            Console.Write("Invalid date. Enter Hire Date (yyyy-mm-dd): ");

        Console.Write("Is Active? (Y/N): ");
        bool isActive = (Console.ReadLine() ?? "").Trim().ToUpper() == "Y";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Employee emp = new Employee
        {
            Id = nextId++,
            FirstName = firstName,
            LastName = lastName,
            Department = department,
            JobTitle = jobTitle,
            MonthlySalary = salary,
            TaxRate = taxRate,
            HireDate = hireDate,
            IsActive = isActive,
            Email = email
        };
        employees.Add(emp);

        Console.WriteLine($"\nEmployee added successfully! (ID: {emp.Id})");
    }

    // ---------- READ ----------
    static void ReadEmployees()
    {
        Console.WriteLine("--- Employee Records ---");
        DisplayEmployees(employees);
    }

    static void DisplayEmployees(List<Employee> list)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        foreach (Employee e in list)
        {
            decimal netPay = e.MonthlySalary * (1 - (decimal)e.TaxRate);
            Console.WriteLine($"ID:          {e.Id}");
            Console.WriteLine($"Name:        {e.FirstName} {e.LastName}");
            Console.WriteLine($"Department:  {e.Department}");
            Console.WriteLine($"Job Title:   {e.JobTitle}");
            Console.WriteLine($"Salary:      {e.MonthlySalary:N2}");
            Console.WriteLine($"Tax Rate:    {e.TaxRate:P0}");
            Console.WriteLine($"Net Pay:     {netPay:N2}");
            Console.WriteLine($"Hire Date:   {e.HireDate:yyyy-MM-dd}");
            Console.WriteLine($"Status:      {(e.IsActive ? "Active" : "On Leave/Inactive")}");
            Console.WriteLine($"Email:       {e.Email}");
            Console.WriteLine(new string('-', 45));
        }
        Console.WriteLine($"Total: {list.Count} employee(s)");
    }

    // ---------- SEARCH / FILTER ----------
    static void SearchEmployees()
    {
        Console.WriteLine("--- Search / Filter ---");
        Console.WriteLine("1. By Name (first or last)");
        Console.WriteLine("2. By Department");
        Console.WriteLine("3. By Status (Active / Inactive)");
        Console.Write("\nChoose a filter: ");
        string? option = Console.ReadLine();
        Console.WriteLine();

        List<Employee> results;

        switch (option)
        {
            case "1":
                Console.Write("Enter name to search: ");
                string name = (Console.ReadLine() ?? "").Trim();
                results = employees
                    .Where(e => e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)
                             || e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case "2":
                Console.Write("Enter department: ");
                string dept = (Console.ReadLine() ?? "").Trim();
                results = employees
                    .Where(e => e.Department.Equals(dept, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                break;
            case "3":
                Console.Write("Show Active employees? (Y/N): ");
                bool active = (Console.ReadLine() ?? "").Trim().ToUpper() == "Y";
                results = employees.Where(e => e.IsActive == active).ToList();
                break;
            default:
                Console.WriteLine("Invalid filter.");
                return;
        }

        Console.WriteLine("\n--- Search Results ---");
        DisplayEmployees(results);
    }

    // ---------- UPDATE ----------
    static void UpdateEmployee()
    {
        ReadEmployees();

        Console.Write("\nEnter the ID of the employee to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Employee? emp = employees.FirstOrDefault(e => e.Id == id);

            if (emp != null)
            {
                Console.WriteLine("(Press Enter to keep the current value)\n");

                Console.Write($"First Name ({emp.FirstName}): ");
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) emp.FirstName = input;

                Console.Write($"Last Name ({emp.LastName}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) emp.LastName = input;

                Console.Write($"Department ({emp.Department}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) emp.Department = input;

                Console.Write($"Job Title ({emp.JobTitle}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) emp.JobTitle = input;

                Console.Write($"Monthly Salary ({emp.MonthlySalary}): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal newSalary) && newSalary >= 0)
                    emp.MonthlySalary = newSalary;

                Console.Write($"Tax Rate ({emp.TaxRate}): ");
                if (double.TryParse(Console.ReadLine(), out double newTax) && newTax >= 0 && newTax <= 1)
                    emp.TaxRate = newTax;

                Console.Write($"Hire Date ({emp.HireDate:yyyy-MM-dd}): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
                    emp.HireDate = newDate;

                Console.Write($"Is Active? Y/N ({(emp.IsActive ? "Y" : "N")}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    emp.IsActive = input.Trim().ToUpper() == "Y";

                Console.Write($"Email ({emp.Email}): ");
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) emp.Email = input;

                Console.WriteLine("\nEmployee updated successfully!");
            }
            else
            {
                Console.WriteLine("\nEmployee not found.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid ID format.");
        }
    }

    // ---------- DELETE ----------
    static void DeleteEmployee()
    {
        ReadEmployees();

        Console.Write("\nEnter the ID of the employee to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            int removed = employees.RemoveAll(e => e.Id == id);

            if (removed > 0)
                Console.WriteLine("\nEmployee deleted successfully!");
            else
                Console.WriteLine("\nEmployee not found.");
        }
        else
        {
            Console.WriteLine("\nInvalid ID format.");
        }
    }
}