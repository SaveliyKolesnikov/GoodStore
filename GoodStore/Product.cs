using System.Collections.Generic;

namespace GoodStore
{
    public class Product
    {
        public Product(string name, double price, string unit, double amount)
            : this(-1, name, price, unit, amount)
        {
        }


        public Product(int productId, string name, double price, string unit, double amount)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Unit = unit;
            Amount = amount;
            Consignments = new HashSet<Consignment>();
        }

        public int ProductId { get; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public double Amount { get; set; }

        public ICollection<Consignment> Consignments { get; set; }

        public override string ToString() =>
            $"{ProductId}. {Name} -- {Price} -- Amount: {Amount} {Unit}";
    }
}
