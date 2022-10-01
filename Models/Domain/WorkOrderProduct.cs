using System;
using System.Collections.Generic;

namespace dotnet_inventory_example
{
    public partial class WorkOrderProduct
    {
        public int WorkOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }
}
