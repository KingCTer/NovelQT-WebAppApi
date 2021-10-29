using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NovelQT.Application.Elasticsearch;
using NovelQT.Application.Elasticsearch.Contracts;
using NovelQT.Application.Elasticsearch.Models;
using NovelQT.Application.Interfaces;
using NovelQT.Application.Query;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Services.Api.Controllers.Base;

namespace NovelQT.Services.Api.Controllers
{
    public class SearchController : MobileApiController
    {
        private readonly ISearchAppService _searchAppService;

        private readonly ElasticsearchClient elasticsearchClient;

        public SearchController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            ElasticsearchClient elasticsearchClient,
            ISearchAppService searchAppService
            ) : base(notifications, mediator)
        {
            this.elasticsearchClient = elasticsearchClient;
            _searchAppService = searchAppService;
        }


        [HttpGet("Book")]
        public async Task<IActionResult> SearchBook([FromQuery] PaginationFilter filter, CancellationToken cancellationToken)
        {
            var responses = await _searchAppService.SearchBookAsync(
                filter.Query,
                (filter.PageNumber - 1) * filter.PageSize,
                filter.PageSize,
                cancellationToken);

            return PagedResponse(responses.SearchResult, (int)responses.TotalRecords, filter);
        }


        [HttpGet("Chapter")]
        public async Task<IActionResult> SearchChapter([FromQuery] PaginationFilter filter, CancellationToken cancellationToken)
        {
            var responses = await _searchAppService.SearchChapterAsync(
                filter.Query,
                (filter.PageNumber - 1) * filter.PageSize,
                filter.PageSize,
                cancellationToken);


            return PagedResponse(responses.SearchResult, (int)responses.TotalRecords, filter);
        }

    }
}
