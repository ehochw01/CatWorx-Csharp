using System;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker {
    class Employee {
        // by default properties defined in a class are private, meaning that can only be accessed within the class
        // must give it the public access modifier
        private string FirstName;
        private string LastName;
        private int Id;
        private string PhotoUrl;
        public Employee(string firstName, string lastName, int id, string photoUrl) {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
            PhotoUrl = photoUrl;
        }
        public string GetFullName() {
            return FirstName + " " + LastName;
        }
        public int GetId() {
            return Id;
        }
        public string GetPhotoUrl() {
            return PhotoUrl;
        }

        public string GetCompanyName() {
            return "Cat Worx";
        }
    }
    class Program {
        async static Task Main(string[] args) {
            Console.Write("Populate with random users?(Y/n)");
            string response = Console.ReadLine() ?? "";
            response = response.ToLower();
            List<Employee> employees;
            if (response == "y" || response == "ye" || response == "yes") {
                employees = await PeopleFetcher.GetFromApi();
                
            } else {
                employees = GetEmployees();
            }
            
            Util.PrintEmployees(employees);
            Util.MakeCSV(employees);
            await Util.MakeBadges(employees);


            static List<Employee> GetEmployees() {
                List<Employee> employees = new List<Employee>();
                // Collect user values until the value is an empty string
                while (true) {
                    Console.WriteLine("Enter first name (leave empty to exit): ");
                    // Get a name from the console and assign it to a variable
                    // uses the null coalescing operator ??
                    // this will check for null and replace the null value with the value after the operator
                    string firstName = Console.ReadLine() ?? "";
                    if (firstName == "") break;
                    Console.Write("Enter last name: ");
                    string lastName = Console.ReadLine() ?? "";
                    Console.Write("Enter ID: ");
                    // ReadLine() only returns string types, so need to cast to int
                    int id = Int32.Parse(Console.ReadLine() ?? "");
                    Console.Write("Enter Photo URL:");
                    string photoUrl = Console.ReadLine() ?? "";
                    Employee currentEmployee = new Employee(firstName, lastName, id, photoUrl);
                    employees.Add(currentEmployee);
                }
                return employees;
            }
        }
    }
}
