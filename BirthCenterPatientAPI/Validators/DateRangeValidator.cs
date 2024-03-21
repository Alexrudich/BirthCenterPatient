using FluentValidation;
using BirthCenterPatientAPI.Models;

namespace BirthCenterPatientAPI.Validators
{
    public class DateRangeValidator : AbstractValidator<DateRange>
    {
        public DateRangeValidator()
        {
            RuleFor(x => x.DateFrom)
                .NotEmpty().WithMessage("DateFrom is required")
                .LessThanOrEqualTo(x => x.DateTo).WithMessage("DateFrom must be less than or equal to DateTo");

            RuleFor(x => x.DateTo)
                .NotEmpty().WithMessage("DateTo is required");
        }
    }
}

