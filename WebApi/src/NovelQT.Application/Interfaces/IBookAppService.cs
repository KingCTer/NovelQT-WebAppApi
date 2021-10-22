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
        public BookViewModel GetById(Guid id);
        public IEnumerable<BookViewModel> GetAll();
        public RepositoryResponses<BookViewModel> GetAll(int skip, int take);
    }
}
