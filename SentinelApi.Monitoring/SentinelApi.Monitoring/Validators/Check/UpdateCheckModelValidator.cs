using FluentValidation;
using SentinelApi.Monitoring.Models.Check;

namespace SentinelApi.Monitoring.Validators.Check;

internal class UpdateCheckModelValidator : AbstractValidator<UpdateCheckModel>
{
    public UpdateCheckModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id має бути натуральним числом.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name є обов'язковим.");

        RuleFor(x => x.EndpointUrl)
            .NotEmpty().WithMessage("EndpointUrl є обов'язковим.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description є обов'язковим.");

        RuleFor(x => x.ProbeType)
            .GreaterThanOrEqualTo(0).WithMessage("ProbeType має бути натуральним числом.");
    }
}
