using System.Text.Json.Serialization;

namespace NovelQT.Application.Elasticsearch.Contracts
{
    public class BookSearchResultDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("matches")]
        public string[] Matches { get; set; }

    }
}
