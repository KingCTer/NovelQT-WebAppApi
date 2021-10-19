using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NovelQT.Application.ViewModels
{
    public class AuthorViewModel
    {
        public AuthorViewModel(string name)
        {
            this.Name = name;
        }

        public AuthorViewModel()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

    }
}
