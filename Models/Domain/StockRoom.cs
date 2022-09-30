using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GridShared.DataAnnotations;

namespace dotnet_inventory_example.Models
{
    public partial class StockRoom
    {
        public StockRoom()
        {
            ProductStocks = new HashSet<ProductStock>();
            WorkOrderSourceRooms = new HashSet<WorkOrder>();
            WorkOrderTargetRooms = new HashSet<WorkOrder>();
        }

        public int StockRoomId { get; set; }
        public string RoomName { get; set; }
        public int? StockBuildingId { get; set; }

        public virtual StockBuilding StockBuilding { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        [NotMapped]
        public virtual ICollection<WorkOrder> WorkOrderSourceRooms { get; set; }
        [NotMapped]
        public virtual ICollection<WorkOrder> WorkOrderTargetRooms { get; set; }
    }
}
