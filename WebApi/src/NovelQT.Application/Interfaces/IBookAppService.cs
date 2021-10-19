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
        public IEnumerable<BookViewModel> GetAll(int skip, int take);
    }
}
