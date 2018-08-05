using System.Collections.Generic;

namespace GoodStoreEntity
{
    public partial class Product
    {
        public Product()
        {
            Consignments = new HashSet<Consignment>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public double Amount { get; set; }

        public ICollection<Consignment> Consignments { get; set; }
    }
}
