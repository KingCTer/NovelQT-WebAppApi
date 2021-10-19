using NovelQT.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Domain.Models
{
    public class Book : Entity
    {
        public Book()
        {
        }

        public Book(Guid id, string name, string key, string cover, string status, int view, int like, Guid authorId, Guid categoryId)
        {
            Id = id;
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorId = authorId;
            CategoryId = categoryId;
        }

        public string Name { get; set; }
        public string Key { get; set; }
        public string Cover { get; set; }
        public string Status { get; set; }
        public int View { get; set; }
        public int Like { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Chapter> Chapters { get; set; }
    }
}
