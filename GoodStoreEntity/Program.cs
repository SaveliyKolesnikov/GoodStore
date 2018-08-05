using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GoodStoreEntity
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

            using (var db = new DbContext())
            {
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
    }
}
