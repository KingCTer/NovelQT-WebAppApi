using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class BookResponse
    {
        public BookResponse()
        {
        }

        public BookResponse(Guid id,
                            string name,
                            string key,
                            string cover,
                            string status,
                            int view,
                            int like,
                            string authorName,
                            string categoryName,
                            int chapterTotal,
                            string intro
            )
        {
            Id = id;
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorName = authorName;
            CategoryName = categoryName;
            ChapterTotal = chapterTotal;
            Intro = intro;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Key { get; set; }
        public string Cover { get; set; }
        public string Status { get; set; }
        public int View { get; set; }
        public int Like { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public int ChapterTotal { get; set; }
        public string Intro { get; set; }
    }
}
