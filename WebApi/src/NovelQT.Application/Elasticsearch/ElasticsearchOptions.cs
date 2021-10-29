namespace NovelQT.Application.Elasticsearch
{
    public class ElasticsearchOptions
    {
        public bool IsUseElasticsearch { get; set; }
        public string Uri { get; set; }

        public bool IsUseCloud { get; set; }
        public string CloudId { get; set; }
        public string CloudUsername { get; set; }
        public string CloudPassword { get; set; }

        public string IndexBook { get; set; }
        public string IndexChapter { get; set; }


        public bool IsLoop { get; set; }
    }
}
