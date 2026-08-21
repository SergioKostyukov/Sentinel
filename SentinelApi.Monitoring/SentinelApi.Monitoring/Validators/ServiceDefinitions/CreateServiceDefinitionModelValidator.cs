using FluentValidation;
using SentinelApi.Monitoring.Models.ServiceDefinition;

namespace SentinelApi.Monitoring.Validators.ServiceDefinition;

internal class CreateServiceDefinitionModelValidator : AbstractValidator<CreateServiceDefinitionModel>
{
    public CreateServiceDefinitionModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name є обов'язковим.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url є обов'язковим.");

        RuleFor(x => x.NotificationEmails)
            .NotEmpty().WithMessage("NotificationEmails є обов'язковим.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description є обов'язковим.");
    }
}
