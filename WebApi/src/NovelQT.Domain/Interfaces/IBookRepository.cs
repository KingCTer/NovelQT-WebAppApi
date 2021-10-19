using NovelQT.Domain.Models;
using NovelQT.Domain.Models.Enum;

namespace NovelQT.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetByKey(string key);
        IQueryable<Book> GetByIndexStatus(IndexStatusEnum indexStatus);
    }
}
