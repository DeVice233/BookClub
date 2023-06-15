using System;
using System.Collections.Generic;

namespace BookClub.Models
{
    public partial class TakePoint
    {
        public TakePoint()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
