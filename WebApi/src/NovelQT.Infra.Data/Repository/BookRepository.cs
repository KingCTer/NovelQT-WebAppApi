using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Infra.Data.Context;
using System.Linq;

namespace NovelQT.Infra.Data.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Book GetByKey(string key)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Key == key);
        }

    }
}
