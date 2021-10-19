using FluentValidation;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Category;
using System;

namespace NovelQT.Domain.Validations.Category
{
    public abstract class CategoryValidation<T> : AbstractValidator<T> where T : CategoryCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }


        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

    }
}
