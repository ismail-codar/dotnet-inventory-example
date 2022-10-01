using System;
using System.Collections.Generic;

namespace dotnet_inventory_example
{
    public partial class StockBuilding
    {
        public StockBuilding()
        {
            StockRooms = new HashSet<StockRoom>();
        }

        public int StockBuildingId { get; set; }
        public string BuildingName { get; set; }

        public virtual ICollection<StockRoom> StockRooms { get; set; }
    }
}
