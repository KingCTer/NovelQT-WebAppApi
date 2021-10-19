using FluentValidation;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Book;
using System;

namespace NovelQT.Domain.Validations.Author;

public abstract class BookValidation<T> : AbstractValidator<T> where T : BookCommand
{
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the Name");
    }


    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

}
