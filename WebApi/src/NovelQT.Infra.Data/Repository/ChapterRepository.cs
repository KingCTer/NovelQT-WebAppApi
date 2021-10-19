using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Infra.Data.Context;
using System.Linq;

namespace NovelQT.Infra.Data.Repository
{
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        public ChapterRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Chapter GetByBookIdAndOrder(Guid bookId, int order)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.BookId == bookId && c.Order == order);
        }


    }
}
