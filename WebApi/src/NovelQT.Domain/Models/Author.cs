using NovelQT.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Domain.Models
{
    public class Author : Entity
    {
        public Author()
        {
        }

        public Author(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
