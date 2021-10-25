using NovelQT.Domain.Models.Enum;
using NovelQT.Domain.Validations.Book;
using System;

namespace NovelQT.Domain.Commands.Book
{
    public class RegisterNewBookCommand : BookCommand
    {
        public RegisterNewBookCommand(string name,
                                      string key,
                                      string cover,
                                      string status,
                                      int view,
                                      int like,
                                      Guid authorId,
                                      Guid categoryId,
                                      IndexStatusEnum indexStatus,
                                      int chapterTotal,
                                      string intro
            )
        {
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorId = authorId;
            CategoryId = categoryId;
            IndexStatus = indexStatus;
            ChapterTotal = chapterTotal;
            Intro = intro;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewBookCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
