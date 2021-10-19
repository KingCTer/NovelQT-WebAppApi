
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using NovelQT.Application.Elasticsearch.Models;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NovelQT.Application.Elasticsearch.Services
{
    public class ElasticsearchIndexService
    {
        private readonly ILogger<ElasticsearchIndexService> logger;
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly IAuthorRepository _authorRepository;

        public ElasticsearchIndexService(
            ILogger<ElasticsearchIndexService> logger, 
            ElasticsearchClient elasticsearchClient,
            IServiceProvider serviceProvider
            )
        {
            this.logger = logger;
            this.elasticsearchClient = elasticsearchClient;
            _authorRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IAuthorRepository>();
        }

        public async Task<IndexResponse> IndexBookAsync(Book document, CancellationToken cancellationToken)
        {
            return await elasticsearchClient.IndexBookAsync(new ElasticsearchBook
            {
                Id = document.Id.ToString(),
                Name = document.Name,
                Author = _authorRepository.GetById(document.AuthorId).Name,
                IndexedOn = DateTime.UtcNow,
            }, cancellationToken);
        }

        public async Task<DeleteResponse> DeleteBookAsync(Book document, CancellationToken cancellationToken)
        {
            return await elasticsearchClient.DeleteBookAsync(document.Id.ToString(), cancellationToken);
        }

        public async Task<IndexResponse> IndexChapterAsync(Chapter document, CancellationToken cancellationToken)
        {
            return await elasticsearchClient.IndexChapterAsync(new ElasticsearchChapter
            {
                Id = document.Id.ToString(),
                BookId = document.BookId.ToString(),
                Order = document.Order,
                Name = document.Name,
                Content = document.Content,
                IndexedOn = DateTime.UtcNow,
            }, cancellationToken);
        }

        public async Task<DeleteResponse> DeleteChapterAsync(Chapter document, CancellationToken cancellationToken)
        {
            return await elasticsearchClient.DeleteChapterAsync(document.Id.ToString(), cancellationToken);
        }


        public async Task<PingResponse> PingAsync(CancellationToken cancellationToken)
        {
            return await elasticsearchClient.PingAsync(cancellationToken: cancellationToken);
        }


    }
}
