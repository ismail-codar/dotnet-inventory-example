using dotnet_inventory_example.Filters;
using dotnet_inventory_example.Models;
using dotnet_inventory_example.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using GridMvc;

namespace dotnet_inventory_example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InventoryDbContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
                // options.UseGridBlazorDatabase();
                //options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
            });

            // Add framework services.
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddGridMvc();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.AddScoped<LanguageFilter>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductUnitService, ProductUnitService>();
            services.AddScoped<IStockBuildingService, StockBuildingService>();
            services.AddScoped<IStockRoomService, StockRoomService>();
            services.AddScoped<IProductStockService, ProductStockService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddScoped<IWorkOrderProductService, WorkOrderProductService>();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("tr-TR"),
                            /*
                            new CultureInfo("de-DE"),
                            new CultureInfo("it-IT"),
                            new CultureInfo("es-ES"),
                            new CultureInfo("fr-FR"),
                            new CultureInfo("ru-RU"),
                            new CultureInfo("nb-NO"),
                            new CultureInfo("nl-NL"),
                            new CultureInfo("tr-TR"),
                            new CultureInfo("cs-CZ"),
                            new CultureInfo("sl-SI"),
                            new CultureInfo("se-SE"),
                            new CultureInfo("sr-Cyrl-RS"),
                            new CultureInfo("hr-HR"),
                            new CultureInfo("fa-IR"),
                            new CultureInfo("ca-ES"),
                            new CultureInfo("gl-ES"),
                            new CultureInfo("eu-ES"),
                            new CultureInfo("pt-BR"),
                            new CultureInfo("bg-BG"),
                            new CultureInfo("uk-UA"),
                            new CultureInfo("ar-EG"),
                            new CultureInfo("da-DK")
                            */
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}