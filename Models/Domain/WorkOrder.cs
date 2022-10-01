using System;
using System.Collections.Generic;

namespace dotnet_inventory_example.Models
{
    public partial class WorkOrder
    {
        public int WorkOrderId { get; set; }
        public int ProductId { get; set; }
        public int? SourceRoomId { get; set; }
        public int? TargetRoomId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public virtual Product Product { get; set; }
        public virtual StockRoom SourceRoom { get; set; }
        public virtual StockRoom TargetRoom { get; set; }
    }
}
