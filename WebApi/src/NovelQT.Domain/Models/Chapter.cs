using NovelQT.Domain.Core.Models;
using NovelQT.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Domain.Models
{
    public class Chapter : Entity
    {
        public Chapter()
        {
        }

        public Chapter(Guid id, int order, string name, string url, string content, Guid bookId, IndexStatusEnum indexStatus)
        {
            Id = id;
            Order = order;
            Name = name;
            Url = url;
            Content = content;
            BookId = bookId;
            IndexStatus = indexStatus;
        }

        public Chapter(Guid id, Guid bookId, int order, string name, string url)
        {
            Id = id;
            Order = order;
            Name = name;
            Url = url;
            BookId = bookId;
        }

        public int Order { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }

        public IndexStatusEnum IndexStatus { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
