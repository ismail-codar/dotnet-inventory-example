using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace dotnet_inventory_example.Models
{
    public partial class ProductStock
    {
        [Key]
        public int StockRoomId { get; set; }
        public int Quantity { get; set; }
        [Key]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual StockRoom StockRoom { get; set; }
    }
}
