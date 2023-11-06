using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Lab_10___Anropa_Databasen.Models;
using Lab_10___Anropa_Databasen.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Lab_10___Anropa_Databasen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (NorthWindContext context = new NorthWindContext())
            {
                Customer customer = StartMenu(context);
            }
        }
        //- Start Menu method
        static Customer StartMenu(NorthWindContext context)
        {
            // Ask the user what to do. Using Q either as a Go Back function or a Quit function.
            Console.WriteLine("Welcome to Database Explorer");
            Console.WriteLine("[1] View Customers");
            Console.WriteLine("[2] Add new Customer");
            Console.WriteLine("[Q] Quit the program");
            Console.Write("Input one of the options above: ");

            string input = Console.ReadLine(); // Stores user input as input

            // Using if to see if the user inputs 1, 2 or Q
            switch (input)
            {
                case "1":
                    Console.Clear();
                    SelectOrder(context);
                    break;

                case "2":
                    Console.WriteLine("Create new Customer");
                    AddCustomer(context);
                    break;

                case "q":
                    Console.WriteLine("Q has been entered, Quitting program....");
                    Environment.Exit(0);
                    break;
            }

            return null;
        }
        //- Select Order method
        static void SelectOrder(NorthWindContext context)
        {
            Console.WriteLine("[1] To display Customers in Ascending order");
            Console.WriteLine("[2] To display Customers in Descending order");
            Console.WriteLine("[Q] To go back to Start Menu");
            Console.Write("Input one of the options above: ");

            //Take users input
            string orderInput = Console.ReadLine();
            switch (orderInput)
            {
                case "1":
                    ListCustomers(context, true); // 
                    break;

                case "2":
                    ListCustomers(context, false);
                    break;

                case "q":
                    Console.Clear();
                    StartMenu(context);
                    break;
            }
        }

        //- Listing all customers method
        static void ListCustomers(NorthWindContext context, bool asc)
        {
            // Checking if (bool asc) is true or false, this changes the OrderBy to either Ascending or Descending
            Console.WriteLine("Here's a list of all customers: ");
            var customerList = asc
                ? context.Customers.OrderBy(c => c.CompanyName).ToList()
                : context.Customers.OrderByDescending(c => c.CompanyName).ToList();

            // Loop through the list and print out a number before every Company Name.
            int i = 0;
            foreach (var customer in customerList)
            {
                i++;
                Console.WriteLine($"{+ i}: {customer.CompanyName}");
            }

            // While loop that runs until Q is entered.
            while (true)
            {
                Console.Write("Enter a Company name you want to inspect or Enter [Q] to go back: ");
                string inspectInput = Console.ReadLine();

                //If the input is Q go back to start menu
                if (inspectInput == "q")
                {
                    Console.WriteLine("Going back...");
                    Console.Clear();
                    StartMenu(context);
                    break;
                }

                // Add the customer to a list called inspection and include Orders so we can see all orders made
                List<Customer> inspection = context.Customers
                        .Where(u => u.CompanyName == inspectInput)
                        .Include(u => u.Orders)
                        .ToList();

                //Listing all the properties except ID for the customer selected.
                if (inspection.Count > 0)
                {
                    foreach (Customer customer in inspection)
                    {
                        Console.WriteLine("\n---\n");
                        Console.WriteLine($"Information about [{customer.CompanyName}]:\n");
                        Console.WriteLine($"Company name: {customer.CompanyName}");
                        Console.WriteLine($"Contact name: {customer.ContactName}");
                        Console.WriteLine($"Contact title: {customer.ContactTitle}");
                        Console.WriteLine($"Address: {customer.Address}");
                        Console.WriteLine($"City: {customer.City}");
                        Console.WriteLine($"Region: {customer.Region}");
                        Console.WriteLine($"Postal Code: {customer.PostalCode}");
                        Console.WriteLine($"Country: {customer.Country}");
                        Console.WriteLine($"Phone: {customer.Phone}");
                        Console.WriteLine($"Fax: {customer.Fax}");
                        Console.WriteLine();
                        Console.WriteLine($"All orders by {customer.CompanyName}:");

                        // Loops through all orders and prints out the info
                        foreach (var order in customer.Orders)
                        {
                            Console.WriteLine($"OrderID: {order.OrderId}, OrderDate: {order.OrderDate}");
                        }
                        Console.WriteLine("\n---\n");
                    }
                }
                // Print this if the company name isn't found.
                else
                {
                    Console.WriteLine("No customers found with the specified company name.");
                }
            }
        }

        //- Add new Customer Method
        static void AddCustomer(NorthWindContext context)
        {
            Console.Clear();
            Console.WriteLine("Adding new Customer...");

            // Take input for every field required. If q is entered it goes back to the Menu
            Console.Write("Enter a Company name (Enter [Q] to Cancel): ");
            string? companyName = Console.ReadLine();
            if (companyName == "q")
            {
                Console.Clear();
                StartMenu(context);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Adding new Customer...");
                Console.WriteLine($"Company name: {companyName}");
                Console.Write("Enter a contact name: ");
                string? contactName = Console.ReadLine();
                Console.Write($"Contact title: ");
                string? contactTitle = Console.ReadLine();
                Console.Write($"Address: ");
                string? address = Console.ReadLine();
                Console.Write($"City: ");
                string? city = Console.ReadLine();
                Console.Write($"Region: ");
                string? region = Console.ReadLine();
                Console.Write($"Postal Code: ");
                string? postalCode = Console.ReadLine();
                Console.Write($"Country: ");
                string? country = Console.ReadLine();
                Console.Write($"Phone: ");
                string? phone = Console.ReadLine();
                Console.Write($"Fax: ");
                string? fax = Console.ReadLine();

                // Below here are 3 different ways to make a CustomerId.

                // Takes the 3 first letters from CompanyName and 2 First letters from ContactName and puts them together as the CustomerId
                // It checks if the strings are same length or longer than required, if its true use Substring otherwise just ToUpper
                // Afterwards add them together as combineId
                string three = companyName.Length >= 3 ? companyName.Substring(0, 3).ToUpper() : companyName.ToUpper();
                string two = contactName.Length >= 2 ? contactName.Substring(0, 2).ToUpper() : contactName.ToUpper();
                string combineId = three + two;

                // Uses the Company name as CustomerID, shortens the input to 5 and makes it uppercase to match all other ID's
                //string fiveId = companyName.Length >= 5 ? companyName.Substring(0, 5).ToUpper() : companyName.ToUpper();

                // Making a Randomly generated CustomerID that is 5 characters
                //string GenerateRandomId(int length)
                //{
                //    const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
                //    var random = new Random();
                //    return new string(Enumerable.Repeat(chars, length)
                //        .Select(s => s[random.Next(s.Length)]).ToArray());
                //}
                //string randomId = GenerateRandomId(5);


                // Takes all the users input, using IsNullOrWhiteSpace to send NULL if empty
                Customer newCustomer = new Customer()
                {
                    CustomerId = combineId, // Using randomId can change to one of the other methods, like fiveId or randomId
                    CompanyName = companyName,
                    ContactName = string.IsNullOrWhiteSpace(contactName) ? null : contactName,
                    ContactTitle = string.IsNullOrWhiteSpace(contactTitle) ? null : contactTitle,
                    Address = string.IsNullOrWhiteSpace(address) ? null : address,
                    City = string.IsNullOrWhiteSpace(city) ? null : city,
                    Region = string.IsNullOrWhiteSpace(region) ? null : region,
                    PostalCode = string.IsNullOrWhiteSpace(postalCode) ? null : postalCode,
                    Country = string.IsNullOrWhiteSpace(country) ? null : country,
                    Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                    Fax = string.IsNullOrWhiteSpace(fax) ? null : fax
                };

                // Saves the inputs to the context 
                context.Customers.Add(newCustomer);
                context.SaveChanges();

                Console.WriteLine($"\nCustomer {companyName} has been added Successfully!\n");
                Console.WriteLine("[1] Add new Customer");
                Console.WriteLine("[2] Go back to menu");
                Console.WriteLine("[Q] Quit the program");
                Console.Write("Input one of the options above: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        AddCustomer(context);
                        break;
                    case "2":
                        Console.Clear();
                        StartMenu(context);
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                }

            }
            
        }

    }
}