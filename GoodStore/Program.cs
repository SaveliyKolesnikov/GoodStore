using System;
using System.IO;
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
            Console.WriteLine(connectionString);
        }
    }
}
