using NovelQT.Domain.Models;

namespace NovelQT.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByName(string name);
    }
}
