using System;

namespace GoodStoreEntity
{
    public partial class Consignment
    {

        public Consignment(int productId, double amount, DateTime date)
        {
            ProductId = productId;
            Amount = amount;
            Date = date;
        }

        public Consignment(int consignmentId, int productId, double amount, DateTime date)
            : this(productId, amount, date)
        {
            ConsignmentId = consignmentId;
        }

        public int ConsignmentId { get; set; }
        public int ProductId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public Product Product { get; set; }
    }
}
