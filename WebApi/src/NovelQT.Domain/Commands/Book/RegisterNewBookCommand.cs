using NovelQT.Domain.Validations.Book;
using System;

namespace NovelQT.Domain.Commands.Book
{
    public class RegisterNewBookCommand : BookCommand
    {
        public RegisterNewBookCommand(string name, string key, string cover, string status, int view, int like, Guid authorId, Guid categoryId)
        {
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorId = authorId;
            CategoryId = categoryId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
