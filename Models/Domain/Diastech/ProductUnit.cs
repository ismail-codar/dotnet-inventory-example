
using System.ComponentModel.DataAnnotations;

namespace dotnet_inventory_example.Models
{
    public partial class ProductUnit
    {
        public ProductUnit()
        {
        }
        [Key]
        public int UnitId { get; set; }
        public string UnitName { get; set; }

        // public virtual ICollection<Product2> Products { get; set; }

    }
}