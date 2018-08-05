using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace GoodStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);

            using (var productRepository = new ProductsRepository(connection))
            using (var consignmentRepository = new ConsignmentRepository(connection))
            {

                bool flag;
                do
                {
                    Console.WriteLine("Input 1 if you want to enter a new product, input 0 to exit.");
                    flag = Convert.ToInt32(Console.ReadLine()) != 0;
                    if (flag)
                    {
                        var newProduct = InputNewProduct();
                        productRepository.CreateProductAsync(newProduct).GetAwaiter().GetResult();
                    }
                } while (flag);

                var products = productRepository.GetProductsAsync().GetAwaiter().GetResult();
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }

                Console.WriteLine(new String('-', 30));
                do
                {
                    Console.WriteLine("Input 1 if you want to enter a new consigment, input 0 to exit.");
                    flag = Convert.ToInt32(Console.ReadLine()) != 0;
                    if (flag)
                    {
                        var newConsignment = InputConsignment(products);
                        consignmentRepository.CreateConsignmentAsync(newConsignment).GetAwaiter().GetResult();
                    }
                } while (!flag);

                Console.WriteLine("Products in the warehouse: ");

                foreach (var product in productRepository.GetProductsAsync().GetAwaiter().GetResult().Where(p => p.Amount > 0))
                    Console.WriteLine(product);
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
