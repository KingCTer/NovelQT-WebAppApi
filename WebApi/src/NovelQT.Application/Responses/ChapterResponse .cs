using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class ChapterResponse
    {
        public ChapterResponse()
        {
        }

        public ChapterResponse(Guid id, Guid bookId, int order, string name, string url, string content)
        {
            Id = id;
            BookId = bookId;
            Order = order;
            Name = name;
            Url = url;
            Content = content;
        }

        public ChapterResponse(Guid id, Guid bookId, int order, string name)
        {
            Id = id;
            BookId = bookId;
            Order = order;
            Name = name;
        }

        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public int Order { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }

    }
}
