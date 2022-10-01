using System;
using System.Collections.Generic;

namespace dotnet_inventory_example
{
    public partial class WorkOrder
    {
        public WorkOrder()
        {
            WorkOrderProducts = new HashSet<WorkOrderProduct>();
        }

        public int WorkOrderId { get; set; }
        public int? SourceRoomId { get; set; }
        public int? TargetRoomId { get; set; }
        public DateTime Date { get; set; }

        public virtual StockRoom SourceRoom { get; set; }
        public virtual StockRoom TargetRoom { get; set; }
        public virtual ICollection<WorkOrderProduct> WorkOrderProducts { get; set; }
    }
}
