using System.Text.RegularExpressions;
using FluentValidation;
using Templater.Application.DTO;

namespace Templater.Application.Validators;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateDto>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .NotNull().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        
        RuleFor(x => x.HtmlContent)
            .NotEmpty().WithMessage("HtmlContent cannot be empty")
            .Must(ContainHtmlTags).WithMessage("HtmlContent must contain at least one HTML tag")
            .Must(ContainModelPlaceholders).WithMessage("HtmlContent must contain at least one Razor placeholder like @Model[\"...\"]");
    }
    
    private bool ContainHtmlTags(string content)
    {
        return Regex.IsMatch(content, "<.*?>");
    }

    private bool ContainModelPlaceholders(string content)
    {
        return Regex.IsMatch(content, @"@Model\["".+?""\]");
    }
}