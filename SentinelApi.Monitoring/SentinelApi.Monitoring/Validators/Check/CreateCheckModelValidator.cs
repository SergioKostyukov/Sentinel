using FluentValidation;
using SentinelApi.Monitoring.Models.Check;

namespace SentinelApi.Monitoring.Validators.Check;

internal class CreateCheckModelValidator : AbstractValidator<CreateCheckModel>
{
    public CreateCheckModelValidator()
    {
        RuleFor(x => x.ServiceDefinitionId)
            .GreaterThanOrEqualTo(0).WithMessage("ServiceDefinitionId має бути натуральним числом.");

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
