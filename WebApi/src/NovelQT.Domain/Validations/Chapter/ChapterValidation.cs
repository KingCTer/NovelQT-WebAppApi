using FluentValidation;
using NovelQT.Domain.Commands.Author;
using System;

namespace NovelQT.Domain.Validations.Chapter
{
    public abstract class ChapterValidation<T> : AbstractValidator<T> where T : ChapterCommand
    {
        protected void ValidateName()
        {
            
        }


        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

    }
}
