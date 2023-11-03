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
                Customer customer = CustomerMenu(context);
            }
        }

        static Customer CustomerMenu(NorthWindContext context)
        {
            //Ask the user what to do.
            Console.WriteLine("Enter 1 to Show a list of all customers ascending");
            Console.WriteLine("Enter 2 to Show a list of all customers ascending");
            Console.WriteLine("Enter 3 to Add a new customer to the list");
            Console.WriteLine("Enter Q to quit the program");
            string input = Console.ReadLine(); //stores user input as input

            //Using if to see if the user inputs 1, 2 or Q
            switch (input)
            {
                case "1":
                    ListCustomers(context);
                    break;

                case "2":
                    ListCustomers(context);
                    break;

                case "3":
                    Console.WriteLine("Create new Customer");
                    break;

                case "q":
                    Console.WriteLine("Q has been entered, Exiting program....");
                    Environment.Exit(0);
                    break;


            

            }

            static void ListCustomers(NorthWindContext context)
            {
                Console.WriteLine("Here's a list of all customers: ");

                //foreach (var customer in context.Customers)
                //{

                //    Console.WriteLine($"{customer.CompanyName}");

                //}

                var orderAsc = context.Customers
                    .GroupBy(c => c.CompanyName)
                    .Select(group => new {Name = group.Key, Count = group.Count() })
                    .OrderBy(c => c.Name)
                    .ToList();
                int i = 0;
                foreach (var orderCount in orderAsc)
                {
                    i++;
                    Console.WriteLine($"{orderCount.Count + i-1}: {orderCount.Name}");
                }

                while (true)
                {
                    Console.WriteLine("Enter a Company name you want to inspect (Enter exit to exit): ");
                    string inspectInput = Console.ReadLine();

                    //If the input is exit the application will stop
                    if (inspectInput == "exit")
                    {
                        Console.WriteLine("Exiting program....");
                        Environment.Exit(0);
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
                            Console.WriteLine();
                            Console.WriteLine($"Information about {inspectInput}");
                            Console.WriteLine("---");
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
                            Console.WriteLine("---");
                            Console.WriteLine("All orders this customer has made:");
                            foreach (var order in customer.Orders)
                            {
                                Console.WriteLine($"OrderID: {order.OrderId}, OrderDate: {order.OrderDate}");
                            }
                            Console.WriteLine("---");
                        }
                    }
                    //Print this if the company name isn't found.
                    else
                    {
                        Console.WriteLine("No customers found with the specified company name.");
                    }
                }
            }
            return null;
        }
    }
}