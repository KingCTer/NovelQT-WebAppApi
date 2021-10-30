using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
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

        private readonly ElasticsearchClient elasticsearchClient;

        private readonly ILogger<DocumentIndexerHostedService> logger;
        private readonly ElasticsearchOptions elasticsearchOptions;
        private readonly ApplicationDbContextFactory applicationDbContextFactory;
        private readonly ElasticsearchIndexService elasticsearchIndexService;

        public DocumentIndexerHostedService(
            ILogger<DocumentIndexerHostedService> logger,
            ApplicationDbContextFactory applicationDbContextFactory,
            IOptions<IndexerOptions> options, 
            ElasticsearchIndexService elasticsearchIndexService,
            ElasticsearchClient elasticsearchClient,
            IOptions<ElasticsearchOptions> elasticsearchOptions
            )
        {
            this.logger = logger;
            this.options = options.Value;
            this.elasticsearchIndexService = elasticsearchIndexService;
            this.applicationDbContextFactory = applicationDbContextFactory;
            this.elasticsearchClient = elasticsearchClient;
            this.elasticsearchOptions = elasticsearchOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            
            if (!elasticsearchOptions.IsUseElasticsearch)
            {
                return;
            }
            var healthTimeout = TimeSpan.FromSeconds(50);

            if (elasticsearchOptions.IsUseCloud)
            {
                ClusterHealthResponse healthResponse = await elasticsearchClient.WaitForClusterHealthAsync(healthTimeout, cancellationToken);
                if (healthResponse.ApiCall.Success == false) return;

            }
            else
            {
                ClusterHealthResponse healthResponse = await elasticsearchClient.WaitForClusterAsync(healthTimeout, cancellationToken);
                if (healthResponse.ApiCall.Success == false) return;
            }


            var indexDelay = TimeSpan.FromSeconds(options.IndexDelay);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"DocumentIndexer is starting with Index Delay: {options.IndexDelay} seconds.");
            }

            cancellationToken.Register(() => logger.LogDebug($"DocumentIndexer background task is stopping."));

            if (!elasticsearchOptions.IsLoop)
            {
                try
                {
                    await IndexDocumentsAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Indexing failed due to an Exception");
                }
            }
            else
            {
                while (!cancellationToken.IsCancellationRequested && elasticsearchOptions.IsLoop)
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
            }

            

            logger.LogDebug($"DocumentIndexer exited the Index Loop.");
        }

        private async Task IndexDocumentsAsync(CancellationToken cancellationToken)
        {
            await IndexScheduledBooks(cancellationToken);
            await SynchIndexBooks(cancellationToken);
            await RemoveDeletedBooks(cancellationToken);
            await SynchRemoveBooks(cancellationToken);
            await IndexScheduledChapters(cancellationToken);
            await SynchIndexChapters(cancellationToken);
            await RemoveDeletedChapters(cancellationToken);
            await SynchRemoveChapters(cancellationToken);

            async Task IndexScheduledBooks(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);


                var documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.ScheduledIndex)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                foreach (Book document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Start indexing Book: {document.Id}");
                    }

                    try
                    {
                        var indexDocumentResponse = await elasticsearchIndexService.IndexBookAsync(document, cancellationToken);

                        if (indexDocumentResponse.IsValid)
                        {
                            if (elasticsearchOptions.IsUseCloud)
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.IndexedInCloud} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                            else
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.IndexedInDocker} where id = {document.Id}", cancellationToken: cancellationToken);
                            }

                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Indexing Document '{document.Id}' failed");

                        await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished indexing Book: {document.Id}");
                    //}
                }

                await transaction.CommitAsync(cancellationToken);
            }

            async Task SynchIndexBooks(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                List<Book> documents;

                if (elasticsearchOptions.IsUseCloud)
                {
                    documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.IndexedInDocker)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }
                else
                {
                    documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.IndexedInCloud)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }

                foreach (Book document in documents)
                {
                    if (logger.IsInformationEnabled())
                    {
                        logger.LogInformation($"Start indexing Book: {document.Id}");
                    }

                    try
                    {
                        var indexDocumentResponse = await elasticsearchIndexService.IndexBookAsync(document, cancellationToken);

                        if (indexDocumentResponse.IsValid)
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Indexed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Indexing Document '{document.Id}' failed");

                        await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished indexing Book: {document.Id}");
                    //}
                }

                await transaction.CommitAsync(cancellationToken);
            }

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
                            if (elasticsearchOptions.IsUseCloud)
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.DeletedInCloud} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                            else
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.DeletedInDocker} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Document '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished Removing Document: {document.Id}");
                    //}

                    await transaction.CommitAsync(cancellationToken);
                }
            }

            async Task SynchRemoveBooks(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                List<Book> documents;

                if (elasticsearchOptions.IsUseCloud)
                {
                    documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.DeletedInDocker)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }
                else
                {
                    documents = await context.Books
                    .Where(x => x.IndexStatus == IndexStatusEnum.DeletedInCloud)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }

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
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Deleted} where id = {document.Id}", cancellationToken: cancellationToken);

                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE books SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Document '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished Removing Document: {document.Id}");
                    //}

                    await transaction.CommitAsync(cancellationToken);
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
                            if (elasticsearchOptions.IsUseCloud)
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.IndexedInCloud} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                            else
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.IndexedInDocker} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
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

                await transaction.CommitAsync(cancellationToken);
            }

            async Task SynchIndexChapters(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                List<Chapter> documents;

                if (elasticsearchOptions.IsUseCloud)
                {
                    documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.IndexedInDocker)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }
                else
                {
                    documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.IndexedInCloud)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }

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
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Indexed} where id = {document.Id}", cancellationToken: cancellationToken);

                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
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

                await transaction.CommitAsync(cancellationToken);
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
                            if (elasticsearchOptions.IsUseCloud)
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.DeletedInCloud} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                            else
                            {
                                await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.DeletedInDocker} where id = {document.Id}", cancellationToken: cancellationToken);
                            }
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Chapter '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished Removing Chapter: {document.Id}");
                    //}
                    await transaction.CommitAsync(cancellationToken);
                }
            }

            async Task SynchRemoveChapters(CancellationToken cancellationToken)
            {
                using var context = applicationDbContextFactory.Create();
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

                List<Chapter> documents;

                if (elasticsearchOptions.IsUseCloud)
                {
                    documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.DeletedInDocker)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }
                else
                {
                    documents = await context.Chapters
                    .Where(x => x.IndexStatus == IndexStatusEnum.DeletedInCloud)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                }

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
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Deleted} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE chapters SET IndexStatus = {IndexStatusEnum.Failed} where id = {document.Id}", cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Removing Chapter '{document.Id}' failed");

                        //await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE documents SET status = {StatusEnum.Failed} where id = {document.Id}");
                    }

                    //if (logger.IsInformationEnabled())
                    //{
                    //    logger.LogInformation($"Finished Removing Chapter: {document.Id}");
                    //}
                    await transaction.CommitAsync(cancellationToken);
                }
            }
        }
    }
}