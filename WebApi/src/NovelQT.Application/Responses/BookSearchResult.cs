using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class BookSearchResult
    {
        public BookResponse BookResult { get; set; }

        public string[] BookNameHighlight { get; set; }
        public string[] AuthorNameHighlight { get; set; }
    }
}
