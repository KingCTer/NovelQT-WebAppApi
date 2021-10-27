using NovelQT.Domain.Models;
using NovelQT.Domain.Specifications;

namespace NovelQT.Domain.Interfaces
{
    public interface IChapterRepository : IRepository<Chapter>
    {
        Chapter GetChapterByBookIdAndOrder(Guid bookId, int order);
        IQueryable<Chapter> GetChapterListByBookId(Guid bookId);
        SpecificationResponse<Chapter> GetPagination(ISpecification<Chapter> spec);
        Chapter GetLastChapterByBookId(Guid bookId);
    }
}
