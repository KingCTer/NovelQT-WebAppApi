using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Infra.Data.Context;
using System.Linq;

namespace NovelQT.Infra.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Category GetByName(string name)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Name == name);
        }
    }
}
