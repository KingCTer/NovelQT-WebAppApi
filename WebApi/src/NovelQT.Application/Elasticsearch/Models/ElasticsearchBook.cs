using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Elasticsearch.Models
{
    public class ElasticsearchBook
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public DateTime IndexedOn { get; set; }

        public Attachment Attachment { get; set; }
    }
}
