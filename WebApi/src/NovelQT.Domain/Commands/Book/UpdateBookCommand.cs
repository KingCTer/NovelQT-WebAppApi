using NovelQT.Domain.Models.Enum;
using NovelQT.Domain.Validations;
using NovelQT.Domain.Validations.Book;
using System;

namespace NovelQT.Domain.Commands.Book
{
    public class UpdateBookCommand : BookCommand
    {
        public UpdateBookCommand(Guid id, string name, string key, string cover, string status, int view, int like, Guid authorId, Guid categoryId, IndexStatusEnum indexStatus)
        {
            Id = id;
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorId = authorId;
            CategoryId = categoryId;
            IndexStatus = indexStatus;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
