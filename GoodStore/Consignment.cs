using System;

namespace GoodStore
{
    public class Consignment
    {

        public Consignment(int productId, double amount, DateTime date)
            : this(-1, productId, amount, date)
        {
        }


        public Consignment(int consignmentId, int productId, double amount, DateTime date)
        {
            ConsignmentId = consignmentId;
            ProductId = productId;
            Amount = amount;
            Date = date;
        }

        public int ConsignmentId { get; }
        public int ProductId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Product Product { get; set; }
    }
}
