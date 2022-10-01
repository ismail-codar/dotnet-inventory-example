using FluentValidation;

namespace dotnet_inventory_example.Models.Validators
{
    public class ProductStockValidator : AbstractValidator<Product>
    {
        public ProductStockValidator()
        {
        }
    }
}