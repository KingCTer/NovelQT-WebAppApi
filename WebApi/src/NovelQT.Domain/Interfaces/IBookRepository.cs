using NovelQT.Domain.Models;

namespace NovelQT.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetByKey(string key);
    }
}
