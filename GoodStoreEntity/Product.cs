using System.Collections.Generic;

namespace GoodStoreEntity
{
    public class Product
    {
        public Product(string name, double price, string unit, double amount)
        {
            Name = name;
            Price = price;
            Unit = unit;
            Amount = amount;
            Consignments = new HashSet<Consignment>();
        }

        public Product(int productId, string name, double price, string unit, double amount)
        : this(name, price, unit, amount)
        {
            ProductId = productId;
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public double Amount { get; set; }

        public ICollection<Consignment> Consignments { get; set; }

        public override string ToString() =>
            $"{ProductId}. {Name} -- {Price} -- Amount: {Amount} {Unit}";

    }
}
