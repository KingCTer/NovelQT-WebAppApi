using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NovelQT.Application.Elasticsearch;
using NovelQT.Application.Elasticsearch.Contracts;
using NovelQT.Application.Elasticsearch.Models;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Services.Api.Controllers.Base;

namespace NovelQT.Services.Api.Controllers
{
    public class SearchController : MobileApiController
    {
        private readonly ElasticsearchClient elasticsearchClient;

        public SearchController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            ElasticsearchClient elasticsearchClient
            ) : base(notifications, mediator)
        {
            this.elasticsearchClient = elasticsearchClient;
        }

        [HttpGet("Books")]
        public async Task<IActionResult> SearchBook([FromQuery(Name = "q")] string query, CancellationToken cancellationToken)
        {
            var searchResponse = await elasticsearchClient.SearchBookAsync(query, cancellationToken);
            var searchResult = ConvertToBookSearchResults(query, searchResponse);

            return Ok(searchResult);
        }

        [HttpGet("Chapters")]
        public async Task<IActionResult> SearchChapter([FromQuery(Name = "q")] string query, CancellationToken cancellationToken)
        {
            var searchResponse = await elasticsearchClient.SearchChapterAsync(query, cancellationToken);
            var searchResult = ConvertToChapterSearchResults(query, searchResponse);

            return Ok(searchResult);
        }

        private static SearchResultsDto<BookSearchResultDto> ConvertToBookSearchResults(string query, ISearchResponse<ElasticsearchBook> searchResponse)
        {
            var searchResults = searchResponse
                // Get the Hits:
                .Hits
                // Convert the Hit into a SearchResultDto:
                .Select(x => new BookSearchResultDto
                {
                    Id = x.Source.Id,
                    Name = x.Source.Name,
                    Author = x.Source.Author,
                    Matches = GetMatches(x.Highlight),
                })
                // And convert to array:
                .ToArray();

            return new SearchResultsDto<BookSearchResultDto>
            {
                Query = query,
                Results = searchResults
            };
        }

        private SearchResultsDto<ChapterSearchResultDto> ConvertToChapterSearchResults(string query, ISearchResponse<ElasticsearchChapter> searchResponse)
        {
            var searchResults = searchResponse
                // Get the Hits:
                .Hits
                // Convert the Hit into a SearchResultDto:
                .Select(x => new ChapterSearchResultDto
                {
                    Id = x.Source.Id,
                    BookId = x.Source.BookId,
                    Order = x.Source.Order,
                    Name = x.Source.Name,
                    Matches = GetMatchesForChapter(x.Highlight),
                })
                // And convert to array:
                .ToArray();

            return new SearchResultsDto<ChapterSearchResultDto>
            {
                Query = query,
                Results = searchResults
            };
        }

        private static string[] GetMatches(IReadOnlyDictionary<string, IReadOnlyCollection<string>> highlight)
        {
            var matchesForName = GetMatchesForField(highlight, "name");
            var matchesForAuthor = GetMatchesForField(highlight, "author");

            return Enumerable
                .Concat(matchesForName, matchesForAuthor)
                .ToArray();
        }

        private static string[] GetMatchesForChapter(IReadOnlyDictionary<string, IReadOnlyCollection<string>> highlight)
        {
            var matchesForName = GetMatchesForField(highlight, "name");
            var matchesForAuthor = GetMatchesForField(highlight, "content");

            return Enumerable
                .Concat(matchesForName, matchesForAuthor)
                .ToArray();
        }

        private static string[] GetMatchesForField(IReadOnlyDictionary<string, IReadOnlyCollection<string>> highlight, string field)
        {
            if (highlight == null)
            {
                return Array.Empty<string>();
            }

            if (highlight.TryGetValue(field, out var matches))
            {
                return matches.ToArray();
            }

            return Array.Empty<string>();
        }
    }
}
