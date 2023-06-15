using System;
using System.Collections.Generic;

namespace BookClub.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int Number { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int Code { get; set; }
        public decimal CostSum { get; set; }
        public decimal DiscountSum { get; set; }
        public int TakePointId { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual TakePoint TakePoint { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
