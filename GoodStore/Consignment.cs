using System;

namespace GoodStoreEntity
{
    public class Consignment
    {
        public Consignment(int consignmentId, int productId, int amount, DateTime date)
        {
            ConsignmentId = consignmentId;
            ProductId = productId;
            Amount = amount;
            Date = date;
        }

        public int ConsignmentId { get; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public Product Product { get; set; }
    }
}
