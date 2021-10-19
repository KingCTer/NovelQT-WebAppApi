using NovelQT.Domain.Models;
using NovelQT.Domain.Models.Enum;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelQT.Application.ViewModels
{
    public class BookViewModel
    {
        public BookViewModel(string name, string key, string cover, string status, int view, int like, Guid authorId, Guid categoryId)
        {
            Name = name;
            Key = key;
            Cover = cover;
            Status = status;
            View = view;
            Like = like;
            AuthorId = authorId;
            CategoryId = categoryId;
        }

        public BookViewModel(Guid id, string name, string key, string cover, string status, int view, int like, Guid authorId, Guid categoryId, IndexStatusEnum indexStatus)
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
            IndexStatus = indexStatus;
        }

        public BookViewModel()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        public string Key { get; set; }
        public string Cover { get; set; }
        public string Status { get; set; }
        public int View { get; set; }
        public int Like { get; set; }

        public IndexStatusEnum IndexStatus { get; set; }

        [ForeignKey("AuthorId")]
        public Guid AuthorId { get; set; }

        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
    }
}
