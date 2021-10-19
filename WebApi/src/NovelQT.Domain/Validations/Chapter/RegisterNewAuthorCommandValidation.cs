using NovelQT.Domain.Commands;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Book;
using NovelQT.Domain.Validations.Author;

namespace NovelQT.Domain.Validations.Chapter
{
    public class RegisterNewChapterCommandValidation : ChapterValidation<RegisterNewChapterCommand>
    {
        public RegisterNewChapterCommandValidation()
        {
            ValidateName();

        }
    }
}
