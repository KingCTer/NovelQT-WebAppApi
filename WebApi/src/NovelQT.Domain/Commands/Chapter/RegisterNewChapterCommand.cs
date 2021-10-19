using NovelQT.Domain.Models.Enum;
using NovelQT.Domain.Validations.Author;
using NovelQT.Domain.Validations.Chapter;
using System;

namespace NovelQT.Domain.Commands.Author
{
    public class RegisterNewChapterCommand : ChapterCommand
    {
        public RegisterNewChapterCommand(Guid bookId, int order, string name, string url, string content, IndexStatusEnum indexStatus)
        {
            BookId = bookId;
            Order = order;
            Name = name;
            Url = url;
            Content = content;
            IndexStatus = indexStatus;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewChapterCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
