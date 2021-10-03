using NovelQT.Domain.Core.Models;
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

        public int Order { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
