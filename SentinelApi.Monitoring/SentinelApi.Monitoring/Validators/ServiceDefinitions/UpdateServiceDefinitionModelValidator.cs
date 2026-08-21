using FluentValidation;
using SentinelApi.Monitoring.Models.ServiceDefinition;

namespace SentinelApi.Monitoring.Validators.ServiceDefinition;

internal class UpdateServiceDefinitionModelValidator : AbstractValidator<UpdateServiceDefinitionModel>
{
    public UpdateServiceDefinitionModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0).WithMessage("Id має бути натуральним числом.");

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
