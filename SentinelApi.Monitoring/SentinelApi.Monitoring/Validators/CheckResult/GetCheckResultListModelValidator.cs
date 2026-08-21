using FluentValidation;
using SentinelApi.Monitoring.Models.CheckResult;

namespace SentinelApi.Monitoring.Validators.CheckResult;

internal class GetCheckResultListModelValidator : AbstractValidator<GetCheckResultListModel>
{
    public GetCheckResultListModelValidator()
    {
        RuleFor(x => x.SearchParam)
            .MaximumLength(255).WithMessage("Пошуковий параметр не може перевищувати 255 символів.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchParam));

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize має бути додатнім числом.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page має бути додатнім числом.");
    }
}
