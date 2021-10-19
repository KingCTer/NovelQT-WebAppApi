using System.Text.Json.Serialization;

namespace NovelQT.Application.Elasticsearch.Contracts
{
    public class SearchResultsDto<TEntity>
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("results")]
        public TEntity[] Results { get; set; }

    }
}
