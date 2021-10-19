using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NovelQT.Application.ViewModels
{
    public class ChapterViewModel
    {
        public ChapterViewModel()
        {
        }

        public ChapterViewModel(int order, string name, string url, string content, Guid bookId)
        {
            Order = order;
            Name = name;
            Url = url;
            Content = content;
            BookId = bookId;
        }

        [Key]
        public Guid Id { get; set; }

        public int Order { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }

        public Guid BookId { get; set; }

    }
}
