using NovelQT.Domain.Models;

namespace NovelQT.Domain.Interfaces
{
    public interface IChapterRepository : IRepository<Chapter>
    {
        Chapter GetByBookIdAndOrder(Guid bookId, int order);
    }
}
