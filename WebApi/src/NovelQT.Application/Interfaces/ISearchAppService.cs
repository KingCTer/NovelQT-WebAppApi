using NovelQT.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Interfaces
{
    public interface ISearchAppService : IDisposable
    {
        Task<SearchResponse<BookSearchResult>> SearchBookAsync(string query, int skip, int take, CancellationToken cancellationToken);

        Task<SearchResponse<ChapterSearchResult>> SearchChapterAsync(string query, int skip, int take, CancellationToken cancellationToken);
    }
}
