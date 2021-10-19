using NovelQT.Domain.Commands;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Book;

namespace NovelQT.Domain.Validations.Author
{
    public class RegisterNewAuthorCommandValidation : AuthorValidation<RegisterNewAuthorCommand>
    {
        public RegisterNewAuthorCommandValidation()
        {
            ValidateName();

        }
    }
}
