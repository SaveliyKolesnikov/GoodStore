using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GoodStoreEntity
{
    class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();

            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;


            using (var db = new ProductsContext(options))
            {
                var flag = true;
                do
                {
                    Console.WriteLine("Input 1 if you want to enter a new product, input 0 to exit.");
                    try
                    {
                        flag = Convert.ToInt32(Console.ReadLine()) != 0;
                        if (flag)
                        {
                            var newProduct = InputNewProduct();
                            db.Products.Add(newProduct);
                            db.SaveChanges();
                        }
                    }
                    catch (FormatException)
                    {
                        // ignored
                    }

                } while (flag);

                var products = db.Products;
                foreach (var product in products)
                    Console.WriteLine(product);

                Console.WriteLine(new String('-', 60));
                do
                {
                    Console.WriteLine("Input 1 if you want to enter a new consigment, input 0 to exit.");
                    try
                    {
                        flag = Convert.ToInt32(Console.ReadLine()) != 0;
                        if (flag)
                        {
                            var newConsignment = InputConsignment(products);
                            db.Consignment.Add(newConsignment);
                            db.SaveChanges();
                        }
                    }
                    catch (FormatException)
                    {
                        // ignored
                    }
                } while (flag);

                Console.WriteLine("Products in the warehouse: ");

                foreach (var product in db.Products.Include(p => p.Consignments))
                {
                    Console.WriteLine($"{product.Name} -- {product.Price} -- {product.Unit} -- {product.Amount}");
                    foreach (var productConsignment in product.Consignments)
                    {
                        Console.WriteLine($"| {productConsignment.Date} -- {productConsignment.Amount}");
                    }
                }

            }
        }


        private static Product InputNewProduct()
        {
            Console.WriteLine("Enter the name of the product");
            var name = Console.ReadLine();
            var price = InputDoubleValue("price");

            Console.WriteLine("Enter the unit of the product");
            var unit = Console.ReadLine();
            var amount = InputDoubleValue("amount");

            return new Product(name, price, unit, amount);
        }

        private static Consignment InputConsignment(IEnumerable<Product> products)
        {
            int choice;
            var productsList = products.ToList();
            do
            {
                var num = 1;
                foreach (var product in productsList)
                    Console.WriteLine($"{num++}. {product}");

                Console.WriteLine("Select the product you want to supply or -1 if you don't want to.");
                choice = Convert.ToInt32(Console.ReadLine());

                if (choice == -1) return null;
            } while (choice < 1 || choice > productsList.Count);

            var selectedProduct = productsList[choice - 1];

            var productId = selectedProduct.ProductId;
            var amount = InputDoubleValue("amount");

            var supplyTime = InputDate();

            return new Consignment(productId, amount, supplyTime);
        }

        private static DateTime InputDate()
        {
            Console.WriteLine("Enter date in this format - yyyy-MM-dd HH:mm:SS, or enter NOW to automatically insert current time");

            DateTime supplyTime;

            var timeFromUser = Console.ReadLine();

            while (!DateTime.TryParse(timeFromUser, out supplyTime))
            {
                if (timeFromUser == "NOW")
                {
                    supplyTime = DateTime.Now;
                    break;
                }

                Console.WriteLine("Incorrect data format");
                timeFromUser = Console.ReadLine();
            }

            return supplyTime;
        }

        private static double InputDoubleValue(string inputName)
        {
            double value;
            var isInputCorrect = true;
            do
            {
                if (!isInputCorrect)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"Enter the {inputName}");
                isInputCorrect = Double.TryParse(Console.ReadLine(), out value);
            } while (!isInputCorrect);

            return value;
        }
    }
}
