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
            // Checking if (bool asc) is true or false to list in correct order.
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
    }
}