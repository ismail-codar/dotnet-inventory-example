using System;
using System.Collections.Generic;

namespace dotnet_inventory_example
{
    public partial class Product
    {
        public Product()
        {
            ProductStocks = new HashSet<ProductStock>();
            WorkOrderProducts = new HashSet<WorkOrderProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitId { get; set; }
        public string Description { get; set; }

        public virtual ProductUnit Unit { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        public virtual ICollection<WorkOrderProduct> WorkOrderProducts { get; set; }
    }
}
