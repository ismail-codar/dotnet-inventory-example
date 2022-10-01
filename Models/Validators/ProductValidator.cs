using FluentValidation;

namespace dotnet_inventory_example.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("You must enter a price");
        }
    }
}