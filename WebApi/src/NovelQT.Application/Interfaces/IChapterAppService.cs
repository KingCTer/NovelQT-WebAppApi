using NovelQT.Application.Responses;
using NovelQT.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Interfaces
{
    public interface IChapterAppService : IDisposable
    {
        public ChapterResponse GetChapterById(Guid id);
        public ChapterResponse GetChapterByBookIdAndOrder(Guid bookId, int order);
        public ChapterResponse GetLastChapterByBookId(Guid bookId);
        public IEnumerable<ChapterResponse> GetChapterListByBookId(Guid bookId);
        public RepositoryResponses<ChapterResponse> GetChapterListByBookId(Guid bookId, int skip, int take, string query);
    }
}
