﻿using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using NovelQT.Application.Elasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Elasticsearch
{
    public class ElasticsearchClient
    {
        private readonly ILogger<ElasticsearchClient> logger;
        private readonly IElasticClient client;
        private readonly string indexBook;
        private readonly string indexChapter;

        public ElasticsearchClient(
            ILogger<ElasticsearchClient> logger,
            IOptions<ElasticsearchOptions> options
            )
            : this(logger, CreateClient(options.Value.Uri), options.Value.IndexBook, options.Value.IndexChapter)
        {

        }

        public ElasticsearchClient(ILogger<ElasticsearchClient> logger, IElasticClient client, string indexBook, string indexChapter)
        {
            this.logger = logger;
            this.indexBook = indexBook;
            this.indexChapter = indexChapter;
            this.client = client;
        }

        private static IElasticClient CreateClient(string uriString)
        {
            var connectionUri = new Uri(uriString);
            var connectionPool = new SingleNodeConnectionPool(connectionUri);
            var connectionSettings = new ConnectionSettings(connectionPool);

            return new ElasticClient(connectionSettings);
        }

        public async Task<ExistsResponse> ExistsBookAsync(CancellationToken cancellationToken)
        {
            var indexExistsResponse = await client.Indices.ExistsAsync(indexBook, ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"ExistsResponse DebugInformation: {indexExistsResponse.DebugInformation}");
            }

            return indexExistsResponse;
        }

        public async Task<ExistsResponse> ExistsChapterAsync(CancellationToken cancellationToken)
        {
            var indexExistsResponse = await client.Indices.ExistsAsync(indexChapter, ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"ExistsResponse DebugInformation: {indexExistsResponse.DebugInformation}");
            }

            return indexExistsResponse;
        }

        public async Task<PingResponse> PingAsync(CancellationToken cancellationToken)
        {
            var pingResponse = await client.PingAsync(ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Ping DebugInformation: {pingResponse.DebugInformation}");
            }

            return pingResponse;
        }

        public async Task<CreateIndexResponse> CreateBookIndexAsync(CancellationToken cancellationToken)
        {
            var createIndexResponse = await client.Indices.CreateAsync(indexBook, descriptor =>
            {
                return descriptor.Map<ElasticsearchBook>(mapping => mapping
                    .Properties(properties => properties
                        .Text(textField => textField.Name(document => document.Id))
                        .Text(textField => textField.Name(document => document.Name))
                        .Text(textField => textField.Name(document => document.Author))
                        .Date(dateField => dateField.Name(document => document.IndexedOn))
                        .Object<Attachment>(attachment => attachment
                            .Name(document => document.Attachment)
                            .Properties(attachmentProperties => attachmentProperties
                                .Text(t => t.Name(n => n.Name))
                                .Text(t => t.Name(n => n.Content))
                                .Text(t => t.Name(n => n.ContentType))
                                .Number(n => n.Name(nn => nn.ContentLength))
                                .Date(d => d.Name(n => n.Date))
                                .Text(t => t.Name(n => n.Author))
                                .Text(t => t.Name(n => n.Title))
                                .Text(t => t.Name(n => n.Keywords))))));
            }, cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"CreateIndexResponse DebugInformation: {createIndexResponse.DebugInformation}");
            }

            return createIndexResponse;
        }

        public async Task<CreateIndexResponse> CreateChapterIndexAsync(CancellationToken cancellationToken)
        {
            var createIndexResponse = await client.Indices.CreateAsync(indexChapter, descriptor =>
            {
                return descriptor.Map<ElasticsearchChapter>(mapping => mapping
                    .Properties(properties => properties
                        .Text(textField => textField.Name(document => document.Id))
                        .Text(textField => textField.Name(document => document.BookId))
                        .Number(textField => textField.Name(document => document.Order))
                        .Text(textField => textField.Name(document => document.Name))
                        .Text(textField => textField.Name(document => document.Content))
                        .Date(dateField => dateField.Name(document => document.IndexedOn))
                        .Object<Attachment>(attachment => attachment
                            .Name(document => document.Attachment)
                            .Properties(attachmentProperties => attachmentProperties
                                .Text(t => t.Name(n => n.Name))
                                .Text(t => t.Name(n => n.Content))
                                .Text(t => t.Name(n => n.ContentType))
                                .Number(n => n.Name(nn => nn.ContentLength))
                                .Date(d => d.Name(n => n.Date))
                                .Text(t => t.Name(n => n.Author))
                                .Text(t => t.Name(n => n.Title))
                                .Text(t => t.Name(n => n.Keywords))))));
            }, cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"CreateIndexResponse DebugInformation: {createIndexResponse.DebugInformation}");
            }

            return createIndexResponse;
        }

        public async Task<DeleteResponse> DeleteBookAsync(string id, CancellationToken cancellationToken)
        {
            var deleteResponse = await client.DeleteAsync<ElasticsearchBook>(id, x => x.Index(indexBook), cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"DeleteResponse DebugInformation: {deleteResponse.DebugInformation}");
            }

            return deleteResponse;
        }

        public async Task<DeleteResponse> DeleteChapterAsync(string id, CancellationToken cancellationToken)
        {
            var deleteResponse = await client.DeleteAsync<ElasticsearchChapter>(id, x => x.Index(indexChapter), cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"DeleteResponse DebugInformation: {deleteResponse.DebugInformation}");
            }

            return deleteResponse;
        }

        public async Task<GetResponse<ElasticsearchBook>> GetBookByIdAsync(string id, CancellationToken cancellationToken)
        {
            var getResponse = await client.GetAsync<ElasticsearchBook>(id, x => x.Index(indexBook), cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"GetResponse DebugInformation: {getResponse.DebugInformation}");
            }

            return getResponse;
        }

        public async Task<GetResponse<ElasticsearchChapter>> GetChapterByIdAsync(string id, CancellationToken cancellationToken)
        {
            var getResponse = await client.GetAsync<ElasticsearchChapter>(id, x => x.Index(indexChapter), cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"GetResponse DebugInformation: {getResponse.DebugInformation}");
            }

            return getResponse;
        }

        public async Task<ClusterHealthResponse> WaitForClusterAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var clusterHealthResponse = await client.Cluster.HealthAsync(selector: cluster => cluster
                .WaitForNodes("1")
                .WaitForActiveShards("1").Timeout(timeout), ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"ClusterHealthResponse DebugInformation: {clusterHealthResponse.DebugInformation}");
            }

            return clusterHealthResponse;
        }

        public async Task<BulkResponse> BulkBookIndexAsync(IEnumerable<ElasticsearchBook> documents, CancellationToken cancellationToken)
        {
            var request = new BulkDescriptor();

            foreach (var document in documents)
            {
                request.Index<ElasticsearchBook>(op => op
                    .Id(document.Id)
                    .Index(indexBook)
                    .Document(document));
            }

            var bulkResponse = await client.BulkAsync(request, cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"BulkResponse DebugInformation: {bulkResponse.DebugInformation}");
            }

            return bulkResponse;
        }

        public async Task<BulkResponse> BulkChapterIndexAsync(IEnumerable<ElasticsearchChapter> documents, CancellationToken cancellationToken)
        {
            var request = new BulkDescriptor();

            foreach (var document in documents)
            {
                request.Index<ElasticsearchChapter>(op => op
                    .Id(document.Id)
                    .Index(indexChapter)
                    .Document(document));
            }

            var bulkResponse = await client.BulkAsync(request, cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"BulkResponse DebugInformation: {bulkResponse.DebugInformation}");
            }

            return bulkResponse;
        }

        public async Task<IndexResponse> IndexBookAsync(ElasticsearchBook document, CancellationToken cancellationToken)
        {
            var indexResponse = await client.IndexAsync(document, x => x
                .Id(document.Id)
                .Index(indexBook)
                , cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"IndexResponse DebugInformation: {indexResponse.DebugInformation}");
            }

            return indexResponse;
        }

        public async Task<IndexResponse> IndexChapterAsync(ElasticsearchChapter document, CancellationToken cancellationToken)
        {
            var indexResponse = await client.IndexAsync(document, x => x
                .Id(document.Id)
                .Index(indexChapter)
                , cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"IndexResponse DebugInformation: {indexResponse.DebugInformation}");
            }

            return indexResponse;
        }

        public Task<ISearchResponse<ElasticsearchBook>> SearchBookAsync(string query, CancellationToken cancellationToken)
        {
            return client.SearchAsync<ElasticsearchBook>(document => document
                // Query this Index:
                .Index(indexBook)
                // Highlight Text Content:
                .Highlight(highlight => highlight
                    .Fields(
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<strong>")
                            .PostTags("</strong>")
                            .FragmentSize(5)
                            .NoMatchSize(5)
                            .NumberOfFragments(5)
                            .Field(x => x.Name),
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<strong>")
                            .PostTags("</strong>")
                            .FragmentSize(5)
                            .NoMatchSize(5)
                            .NumberOfFragments(5)
                            .Field(x => x.Author))
                    )
                // Now kick off the query:
                .Query(q => q.MultiMatch(mm => mm
                    .Query(query)
                    .Type(TextQueryType.BoolPrefix)
                    .Fields(f => f
                        .Field(d => d.Name)
                        .Field(d => d.Attachment.Content)))), cancellationToken);
        }

        public Task<ISearchResponse<ElasticsearchChapter>> SearchChapterAsync(string query, CancellationToken cancellationToken)
        {
            return client.SearchAsync<ElasticsearchChapter>(document => document
                // Query this Index:
                .Index(indexChapter)
                // Highlight Text Content:
                .Highlight(highlight => highlight
                    .Fields(
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<strong>")
                            .PostTags("</strong>")
                            .FragmentSize(5)
                            .NoMatchSize(5)
                            .NumberOfFragments(5)
                            .Field(x => x.Name),
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<strong>")
                            .PostTags("</strong>")
                            .FragmentSize(150)
                            .NoMatchSize(150)
                            .NumberOfFragments(5)
                            .Field(x => x.Content))
                    )
                // Now kick off the query:
                .Query(q => q.MultiMatch(mm => mm
                    .Query(query)
                    .Type(TextQueryType.BoolPrefix)
                    .Fields(f => f
                        .Field(d => d.Name)
                        .Field(d => d.Attachment.Content)))), cancellationToken);
        }
    }
}
