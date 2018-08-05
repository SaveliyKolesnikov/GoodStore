using System;
using System.Collections.Generic;

namespace GoodStoreEntity
{
    public partial class Consignment
    {
        public int ConsignmentId { get; set; }
        public int ProductId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public Product Product { get; set; }
    }
}
