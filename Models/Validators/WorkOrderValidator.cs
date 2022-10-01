using FluentValidation;

namespace dotnet_inventory_example.Models.Validators
{
    public class WorkOrderValidator : AbstractValidator<WorkOrder>
    {
        public WorkOrderValidator()
        {
            RuleFor(p => p.Date).NotEmpty().WithMessage("You must enter a work order date");
        }
    }
}