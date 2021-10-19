using NovelQT.Domain.Validations.Author;
using System;

namespace NovelQT.Domain.Commands.Category
{
    public class RegisterNewCategoryCommand : CategoryCommand
    {
        public RegisterNewCategoryCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewCategoryCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
