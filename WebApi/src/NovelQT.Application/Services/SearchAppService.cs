using AutoMapper;
using NovelQT.Application.Elasticsearch;
using NovelQT.Application.Interfaces;
using NovelQT.Application.Responses;
using NovelQT.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Services
{
    public class SearchAppService : ISearchAppService
    {
        private readonly IMapper _mapper;

        private readonly ElasticsearchClient _elasticsearchClient;

        private readonly IBookRepository _bookRepository;

        public SearchAppService(
            IMapper mapper,
            ElasticsearchClient elasticsearchClient,
            IBookRepository bookRepository
            )
        {
            _mapper = mapper;
            _elasticsearchClient = elasticsearchClient;
            _bookRepository = bookRepository;
        }

        public async Task<SearchResponse<BookSearchResult>> SearchBookAsync(string query, int skip, int take, CancellationToken cancellationToken)
        {
            if (query == "" || query == null)
            {
                return new SearchResponse<BookSearchResult>(null, 0);
            }
            var searchResponse = await _elasticsearchClient.SearchBookAsync(query, skip, take, cancellationToken);
            var searchResult = searchResponse
                // Get the Hits:
                .Hits
                // Convert the Hit into a SearchResultDto:
                .Select(x => new BookSearchResult
                {
                    BookResult = new BookResponse(Guid.Parse(x.Source.Id), x.Source.Name, x.Source.Author),
                    BookNameHighlight = GetMatchesForField(x.Highlight, "name"),
                    AuthorNameHighlight = GetMatchesForField(x.Highlight, "author")
                })
                // And convert to array:
                .ToArray();

            foreach (var item in searchResult)
            {
                var bookBookResponse = _mapper.Map<BookResponse>(_bookRepository.GetById(item.BookResult.Id));
                bookBookResponse.Intro = null;
                bookBookResponse.AuthorName = item.BookResult.AuthorName;
                item.BookResult = bookBookResponse;
            }
            
            return new SearchResponse<BookSearchResult>(searchResult, searchResponse.Total);

        }

        public async Task<SearchResponse<ChapterSearchResult>> SearchChapterAsync(string query, int skip, int take, CancellationToken cancellationToken)
        {
            if (query is "" or null) return new SearchResponse<ChapterSearchResult>(null, 0);
            
            var searchResponse = await _elasticsearchClient.SearchChapterAsync(query, skip, take, cancellationToken);
            var searchResult = searchResponse
                // Get the Hits:
                .Hits
                
                // Convert the Hit into a SearchResultDto:
                .Select(x => new ChapterSearchResult
                {
                    ChapterResult = new ChapterResponse(
                        Guid.Parse(x.Source.Id),
                        Guid.Parse(x.Source.BookId),
                        x.Source.Order,
                        x.Source.Name
                        ),
                    ContentHighlight = GetMatchesForField(x.Highlight, "content")
                })
                // And convert to array:
                .ToArray();

            foreach (var item in searchResult)
            {
                var bookBookResponse = _mapper.Map<BookResponse>(_bookRepository.GetById(item.ChapterResult.BookId));
                bookBookResponse.Intro = null;
                item.BookResult = bookBookResponse;
            }

            return new SearchResponse<ChapterSearchResult>(searchResult, searchResponse.Total);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        
    }
}
