using System;
using System.Collections.Generic;

namespace dotnet_inventory_example.Models
{
    public partial class ProductStock
    {
        public int Id { get; set; }
        public int StockRoomId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }

        public virtual Product2 Product { get; set; }
        public virtual StockRoom StockRoom { get; set; }
    }
}
