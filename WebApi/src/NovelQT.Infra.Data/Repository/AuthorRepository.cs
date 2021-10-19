using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Infra.Data.Context;
using System.Linq;

namespace NovelQT.Infra.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Author GetByName(string name)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Name == name);
        }

    }
}
