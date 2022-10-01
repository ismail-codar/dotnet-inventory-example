using GridCore.Pagination;
using GridCore.Resources;
using dotnet_inventory_example.Models;
using dotnet_inventory_example.Resources;
using GridMvc.Server;
using GridShared;
using GridShared.Filtering;
using GridShared.Sorting;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Controllers
{
    public class HomeController : ApplicationController
    {

        public HomeController(InventoryDbContext context, ICompositeViewEngine compositeViewEngine) : base(compositeViewEngine)
        {
        }

        public ActionResult Index(string gridState = "")
        {
            //string returnUrl = Request.Path;
            string returnUrl = "/Home/Index";

            IQueryCollection query = Request.Query;
            if (!string.IsNullOrWhiteSpace(gridState))
            {
                try
                {
                    query = new QueryCollection(StringExtensions.GetQuery(gridState));
                }
                catch (Exception)
                {
                    // do nothing, gridState was not a valid state
                }
            }

            ViewBag.ActiveMenuTitle = "Demo";

            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var locale = requestCulture.RequestCulture.UICulture.TwoLetterISOLanguageName;
            SharedResource.Culture = requestCulture.RequestCulture.UICulture;


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
            catch (Exception)
            {
            }
            return LocalRedirect(returnUrl);
        }

    }
}