using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelQT.Application.Elasticsearch.Services;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Commands.Book;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using NovelQT.Domain.Models.Enum;
using NovelQT.Infra.Data.Context;
using NovelQT.Infra.Data.Repository;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovelQT.Application.Elasticsearch.Hosting
{
    public class DocumentIndexerHostedService : BackgroundService
    {
        private readonly IndexerOptions options;

        private readonly ILogger<DocumentIndexerHostedService> logger;
        private readonly ApplicationDbContextFactory applicationDbContextFactory;
        private readonly ElasticsearchIndexService elasticsearchIndexService;

        public DocumentIndexerHostedService(
            ILogger<DocumentIndexerHostedService> logger,
            ApplicationDbContextFactory applicationDbContextFactory,
            IOptions<IndexerOptions> options, 
            ElasticsearchIndexService elasticsearchIndexService
            )
        {
            this.logger = logger;
            this.options = options.Value;
            this.elasticsearchIndexService = elasticsearchIndexService;
            this.applicationDbContextFactory = applicationDbContextFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var indexDelay = TimeSpan.FromSeconds(options.IndexDelay);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"DocumentIndexer is starting with Index Delay: {options.IndexDelay} seconds.");
            }

            cancellationToken.Register(() => logger.LogDebug($"DocumentIndexer background task is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                if (logger.IsDebugEnabled())
                {
                    logger.LogDebug($"DocumentIndexer is running indexing loop.");
                }

                try
                {
                    await IndexDocumentsAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Indexing failed due to an Exception");
                }

                await Task.Delay(indexDelay, cancellationToken);
            }

            logger.LogDebug($"DocumentIndexer exited the Index Loop.");
        }

        private async Task IndexDocumentsAsync(CancellationToken cancellationToken)
        {
            await IndexScheduledBooks(cancellationToken);
            await RemoveDeletedBooks(cancellationToken);
            await IndexScheduledChapters(cancellationToken);
            await RemoveDeletedChapters(cancellationToken);

            async Task RemoveDeletedBooks(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                var documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.ScheduledDelete)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (Book document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Removing Document: {document.Id}");
                    }

                    try
                    {
                        var deleteDocumentResponse = await elasticsearchIndexService.DeleteBookAsync(document, cancellationToken);

                        if (deleteDocumentResponse.IsValid)
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Deleted} where id = {document.Id}");
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Document '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Finished Removing Document: {document.Id}");
                    }
                }
            }

            async Task IndexScheduledBooks(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync();

                var documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.ScheduledIndex)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (Book document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Start indexing Document: {document.Id}");
                    }

                    try
                    {
                        var indexDocumentResponse = await elasticsearchIndexService.IndexBookAsync(document, cancellationToken);

                        if (indexDocumentResponse.IsValid)
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Indexed} where id = {document.Id}");


                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Indexing Document '{document.Id}' failed");

                        await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                    }

                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Finished indexing Document: {document.Id}");
                    }
                }

                await transaction.CommitAsync();
            }

            async Task RemoveDeletedChapters(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                var documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.ScheduledDelete)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (Chapter document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Removing Chapter: {document.Id}");
                    }

                    try
                    {
                        var deleteDocumentResponse = await elasticsearchIndexService.DeleteChapterAsync(document, cancellationToken);

                        if (deleteDocumentResponse.IsValid)
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Deleted} where id = {document.Id}");
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Chapter '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Finished Removing Chapter: {document.Id}");
                    }
                }
            }

            async Task IndexScheduledChapters(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                var documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.ScheduledIndex)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (Chapter document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Start indexing Chapter: {document.Id}");
                    }

                    try
                    {
                        var indexDocumentResponse = await elasticsearchIndexService.IndexChapterAsync(document, cancellationToken);

                        if (indexDocumentResponse.IsValid)
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Indexed} where id = {document.Id}");


                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Indexing Chapter '{document.Id}' failed");

                        await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}");
                    }

                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Finished indexing Chapter: {document.Id}");
                    }
                }

                await transaction.CommitAsync();
            }
        }
    }
}