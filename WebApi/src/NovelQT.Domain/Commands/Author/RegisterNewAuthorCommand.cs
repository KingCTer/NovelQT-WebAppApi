using NovelQT.Domain.Validations.Author;
using System;

namespace NovelQT.Domain.Commands.Author
{
    public class RegisterNewAuthorCommand : AuthorCommand
    {
        public RegisterNewAuthorCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewAuthorCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
