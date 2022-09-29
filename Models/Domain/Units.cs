
using System.ComponentModel.DataAnnotations;

namespace dotnet_inventory_example.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Units
    {
        public Units()
        {
        }
        [Key]
        public int UnitID { get; set; }
        public string UnitName { get; set; }
    }
}