using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Elasticsearch.Models
{
    public class ElasticsearchChapter
    {
        public string Id { get; set; }

        public string BookId { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime IndexedOn { get; set; }

        public Attachment Attachment { get; set; }

    }
}
