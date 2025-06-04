using FluentValidation;
using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100);

            RuleFor(x => x.PublicationYear)
                .GreaterThan(0).WithMessage("Publication year must be greater than 0.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .MaximumLength(100);
        }
    }
} 