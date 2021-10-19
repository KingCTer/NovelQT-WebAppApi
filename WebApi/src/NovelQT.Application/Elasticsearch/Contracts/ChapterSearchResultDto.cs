using System.Text.Json.Serialization;

namespace NovelQT.Application.Elasticsearch.Contracts
{
    public class ChapterSearchResultDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("bookId")]
        public string BookId { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("matches")]
        public string[] Matches { get; set; }

    }
}
