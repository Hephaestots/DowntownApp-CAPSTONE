using Application.DTOs;
using Application.DTOs.Commands;
using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// QuestionTypeDto Validation Class.
    /// </summary>
    public class QuestionTypeCommandDtoValidator : AbstractValidator<QuestionTypeCommandDto>
    {
        /// <summary>
        /// Constructor initialize the validation rules.
        /// </summary>
        public QuestionTypeCommandDtoValidator()
        {
            RuleFor(ec => ec.Name)
                .MaximumLength(25)
                .MinimumLength(5)
                .NotEmpty()
                .WithMessage("The type name is required and must be between 5 and 25 caracters.");

            RuleFor(ec => ec.Description)
                .MaximumLength(255)
                .NotEmpty()
                .WithMessage("The type description is required and is at most 255 caracters.");
        }
    }
}
