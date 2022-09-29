using dotnet_inventory_example.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dotnet_inventory_example.Pages
{
    public class ProductsPageModel : PageModel
    {
        public ProductsPageModel(NorthwindDbContext context)
        {
        }

    }
}
