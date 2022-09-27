using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiasInventory.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The Display Order Must Be Between (1,100)")]
        public int DisplayOrder { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
