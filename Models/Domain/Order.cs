//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GridShared.Columns;
using GridShared.DataAnnotations;
using GridShared.Sorting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dotnet_inventory_example.Models
{
    public class OrderMetaData
    {
        [Display(Name = "Ship name", AutoGenerateFilter = true)]
        public string ShipName { get; set; }
    }

    [ModelMetadataTypeAttribute(typeof(OrderMetaData))]
    //[GridTable(PagingEnabled = true, PageSize = 20)]
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        [GridHiddenColumn]
        [GridColumn(Position = 0)]
        public int OrderID { get; set; }
        [GridColumn(Position = 5)]
        public string CustomerID { get; set; }
        [GridColumn(Position = 4)]
        public int? EmployeeID { get; set; }
        [GridColumn(Position = 1, Title = "Date", Width = "120px", Format = "{0:yyyy-MM-dd}", SortEnabled = true, FilterEnabled = true, SortInitialDirection = GridSortDirection.Descending)]
        public DateTime? OrderDate { get; set; }
        [GridColumn(Position = 2, Width = "120px", Format = "{0:yyyy-MM-dd}")]
        public DateTime? RequiredDate { get; set; }
        [GridColumn(Position = 3, Width = "120px", Format = "{0:yyyy-MM-dd}")]
        public DateTime? ShippedDate { get; set; }
        [GridColumn(Position = 6)]
        public int? ShipVia { get; set; }
        [GridColumn(Position = 7, Title = "Freight", Width = "120px", SortEnabled = true, FilterEnabled = true, AutocompleteTaxonomy = AutoCompleteTerm.Defeat)]
        public decimal? Freight { get; set; }
        [GridColumn(Position = 8)]
        public string ShipName { get; set; }
        [GridColumn(Position = 9)]
        public string ShipAddress { get; set; }
        [GridColumn(Position = 10)]
        public string ShipCity { get; set; }
        [GridColumn(Position = 11)]
        public string ShipRegion { get; set; }
        [GridColumn(Position = 12)]
        public string ShipPostalCode { get; set; }
        [GridColumn(Position = 13, AutocompleteTaxonomy = AutoCompleteTerm.Country)]
        public string ShipCountry { get; set; }
        [NotMappedColumn]
        [GridColumn(Position = 14)]
        public virtual Customer Customer { get; set; }
        [NotMappedColumn]
        [GridColumn(Position = 15)]
        public virtual Employee Employee { get; set; }
        [JsonIgnore]
        [NotMappedColumn]
        [GridColumn(Position = 17)]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [ForeignKey("ShipVia")]
        [NotMappedColumn]
        [GridColumn(Position = 16)]
        public virtual Shipper Shipper { get; set; }
    }

}