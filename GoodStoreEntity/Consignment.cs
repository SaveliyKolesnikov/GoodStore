using System;

namespace GoodStoreEntity
{
    public class Consignment
    {
        public int ConsigmentId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public Product Product { get; set; }
    }
}
