using NovelQT.Application.Responses;
using NovelQT.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Interfaces
{
    public interface IBookAppService : IDisposable
    {
        public void Crawl(string url);
        public BookResponse GetById(Guid id);
        public IEnumerable<BookResponse> GetAll();
        public RepositoryResponses<BookResponse> GetAll(int skip, int take, string query);
    }
}
