using dotnet_inventory_example.Models;
using GridShared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Components
{
    public class ButtonCellViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(object Item, IGrid Grid, object Object)
        {
            int orderId = ((Order)Item).OrderID;
            ViewData["gridState"] = Grid.GetState();
            ViewData["returnUrl"] = (string)Object;

            var factory = Task<IViewComponentResult>.Factory;
            return await factory.StartNew(() => View(orderId));
        }
    }
}
