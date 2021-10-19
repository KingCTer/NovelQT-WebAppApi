using NovelQT.Domain.Commands.Book;
using NovelQT.Domain.Validations.Author;

namespace NovelQT.Domain.Validations.Book
{
    public class UpdateBookCommandValidation : BookValidation<UpdateBookCommand>
    {
        public UpdateBookCommandValidation()
        {
            ValidateId();
            ValidateName();
        }
    }
}
