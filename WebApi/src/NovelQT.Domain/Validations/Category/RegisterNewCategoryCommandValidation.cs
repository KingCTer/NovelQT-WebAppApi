using NovelQT.Domain.Commands.Category;
using NovelQT.Domain.Validations.Category;

namespace NovelQT.Domain.Validations.Author
{
    public class RegisterNewCategoryCommandValidation : CategoryValidation<RegisterNewCategoryCommand>
    {
        public RegisterNewCategoryCommandValidation()
        {
            ValidateName();

        }
    }
}
