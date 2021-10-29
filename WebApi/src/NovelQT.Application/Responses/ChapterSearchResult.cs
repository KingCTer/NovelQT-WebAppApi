using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class ChapterSearchResult
    {
        public ChapterResponse ChapterResult { get; set; }

        public BookResponse BookResult { get; set; }

        public string[] ContentHighlight { get; set; }
    }
}
