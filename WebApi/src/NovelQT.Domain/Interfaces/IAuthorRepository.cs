using NovelQT.Domain.Models;

namespace NovelQT.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Author GetByName(string name);
    }
}
