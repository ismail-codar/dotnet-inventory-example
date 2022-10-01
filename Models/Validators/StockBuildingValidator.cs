using FluentValidation;

namespace dotnet_inventory_example.Models.Validators
{
    public class StockBuildingValidator : AbstractValidator<StockBuilding>
    {
        public StockBuildingValidator()
        {
            RuleFor(p => p.BuildingName).NotEmpty().WithMessage("You must enter a building name");
        }
    }
}