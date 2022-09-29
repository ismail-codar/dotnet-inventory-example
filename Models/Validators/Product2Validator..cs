using FluentValidation;

namespace dotnet_inventory_example.Models.Validators
{
    public class Product2Validator : AbstractValidator<Product2>
    {
        public Product2Validator()
        {
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("You must enter a price");
            RuleFor(p => p.UnitsInStock).NotEmpty().WithMessage("You must enter an units in stock value");
        }
    }
}