
using NovelQT.Domain.Models;

namespace NovelQT.Domain.Specifications
{
    public class BookFilterPaginatedSpecification : BaseSpecification<Book>
    {
        public BookFilterPaginatedSpecification(int skip, int take)
            : base(i => true)
        {
            ApplyPaging(skip, take);
        }
    }
}
