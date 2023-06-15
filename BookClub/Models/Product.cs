using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookClub.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Discount { get; set; }
        public decimal Cost { get; set; }
        public string Photo { get; set; } = null!;

        [NotMapped]
        public string PhotoPath { get => $"\\Images\\{Photo}"; }
        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
