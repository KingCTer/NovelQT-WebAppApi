using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NovelQT.Application.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel(string name)
        {
            this.Name = name;
        }

        public CategoryViewModel()
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
