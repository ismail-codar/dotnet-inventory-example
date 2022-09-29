
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
        public int UnitId { get; set; }
        public string UnitName { get; set; }

        public virtual ICollection<Product2> Products { get; set; }

    }
}