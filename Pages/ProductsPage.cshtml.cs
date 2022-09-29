using GridBlazor.Resources;
using GridCore;
using dotnet_inventory_example.Components;
using dotnet_inventory_example.Models;
using dotnet_inventory_example.Resources;
using dotnet_inventory_example.Services;
using GridMvc.Server;
using GridShared;
using GridShared.Filtering;
using GridShared.Sorting;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using GridBlazor;

namespace dotnet_inventory_example.Pages
{
    public class ProductsPageModel : PageModel
    {
        private readonly ProductsRepository2 _productRepository2;
        private readonly Product2Service _product2Service;

        public ProductsPageModel(NorthwindDbContext context)
        {
            _productRepository2 = new ProductsRepository2(context);
            _product2Service = new Product2Service(new DbContextOptions<NorthwindDbContext>());
        }

        public ISGrid<Product2> Grid { get; set; }

        public IActionResult OnGet(string gridState = "")
        {
            string returnUrl = "/ProductsPage";

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

            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var locale = requestCulture.RequestCulture.UICulture.TwoLetterISOLanguageName;
            SharedResource.Culture = requestCulture.RequestCulture.UICulture;

            Action<IGridColumnCollection<Product2>> columns = c =>
            {
                /* Adding not mapped column, that renders body, using inline Razor html helper */
                c.Add()
                    .Encoded(false)
                    .Sanitized(false)
                    .SetWidth(60)
                    .Css("hidden-xs") //hide on phones
                    .RenderComponentAs<ButtonCellViewComponent>(returnUrl);

                /* Adding "Product2ID" column: */

                c.Add(o => o.ProductId)
                    .Titled(SharedResource.Number)
                    .SetWidth(100)
                    .Sum(true);


                /* Adding "UnitName" column: */
                // c.Add(o => o.Unit.UnitName)
                //     .Titled(SharedResource.CompanyName)
                //     .ThenSortByDescending(o => o.Unit)
                //     .SetWidth(250)
                //     .SetInitialFilter(GridFilterType.StartsWith, "a")
                //     .SetFilterWidgetType("CustomCompanyNameFilterWidget")
                //     .Max(true).Min(true);

            };

            var server = new GridServer<Product2>(_productRepository2.GetAll(), query, false, "Product2sGrid",
                columns, 10, locale)
                // .SetRowCssClasses(item => item.Customer.IsVip ? "success" : string.Empty)
                .Sortable()
                .Filterable()
                .WithMultipleFilters()
                .Searchable(true, false)
                .Groupable(true)
                .ClearFiltersButton(true)
                .Selectable(true)
                .SetStriped(true)
                .ChangePageSize(true)
                .WithGridItemsCount()
                .SetTableLayout(TableLayout.Fixed, "1000px", "400px")
                .SetRemoveDiacritics<NorthwindDbContext>("RemoveDiacritics")
                .SetToListAsyncFunc(async x => await x.ToListAsync());

            Grid = server.Grid;

            return Page();
        }
    }
}
