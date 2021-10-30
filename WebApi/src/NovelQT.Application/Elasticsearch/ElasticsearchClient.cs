using Elasticsearch.Net;
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
            : this(logger, CreateClient(options.Value), options.Value.IndexBook, options.Value.IndexChapter)
        {

        }

        public ElasticsearchClient(ILogger<ElasticsearchClient> logger, IElasticClient client, string indexBook, string indexChapter)
        {
            this.logger = logger;
            this.indexBook = indexBook;
            this.indexChapter = indexChapter;
            this.client = client;
        }

        private static IElasticClient CreateClient(ElasticsearchOptions options)
        {
            var connectionUri = new Uri(options.Uri);
            var connectionPool = new SingleNodeConnectionPool(connectionUri);
            var connectionSettings = new ConnectionSettings(connectionPool);

            if (options.IsUseCloud)
            {
                var cloudId = options.CloudId;
                var credentials = new BasicAuthenticationCredentials(options.CloudUsername, options.CloudPassword);
                var pool = new CloudConnectionPool(cloudId, credentials);

                return new ElasticClient(new ConnectionSettings(pool).UnsafeDisableTls13());
            }

            return new ElasticClient(connectionSettings);
        }

        public async Task<ExistsResponse> ExistsBookAsync(CancellationToken cancellationToken)
        {
            var indexExistsResponse = await client.Indices.ExistsAsync(indexBook, ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Exists Book: {indexExistsResponse.Exists}");
            }

            return indexExistsResponse;
        }

        public async Task<ExistsResponse> ExistsChapterAsync(CancellationToken cancellationToken)
        {
            var indexExistsResponse = await client.Indices.ExistsAsync(indexChapter, ct: cancellationToken);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Exists Chapter: {indexExistsResponse.Exists}");
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
                logger.LogDebug($"CreateIndex Book: {createIndexResponse.ApiCall.Success}");
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
                logger.LogDebug($"CreateIndex Chapter: {createIndexResponse.ApiCall.Success}");
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

            

            return clusterHealthResponse;
        }

        public async Task<ClusterHealthResponse> WaitForClusterHealthAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var clusterHealthResponse = await client.Cluster.HealthAsync(selector: cluster =>
                cluster.Timeout(timeout), ct: cancellationToken);

            

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

            //if (logger.IsDebugEnabled())
            //{
            //    logger.LogDebug($"IndexResponse DebugInformation: {indexResponse.IsValid}");
            //}

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
                logger.LogDebug($"IndexResponse DebugInformation: {indexResponse.IsValid}");
            }

            return indexResponse;
        }

        public Task<ISearchResponse<ElasticsearchBook>> SearchBookAsync(string query, int skip, int take, CancellationToken cancellationToken)
        {
            return client.SearchAsync<ElasticsearchBook>(document => document
                // Query this Index:
                .Index(indexBook)
                // Paginate
                .From(skip)
                .Size(take)
                // Highlight Text Content:
                .Highlight(highlight => highlight
                    .Fields(
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<font color='red'>")
                            .PostTags("</font>")
                            .FragmentSize(150)
                            .NoMatchSize(150)
                            .NumberOfFragments(1)
                            .Field(x => x.Name),
                        fields => fields
                            .Fragmenter(HighlighterFragmenter.Span)
                            .PreTags("<font color='red'>")
                            .PostTags("</font>")
                            .FragmentSize(150)
                            .NoMatchSize(150)
                            .NumberOfFragments(1)
                            .Field(x => x.Author))
                    )
                // Now kick off the query:
                .Query(q => q.MultiMatch(mm => mm
                    .Query(query)
                    .Type(TextQueryType.BestFields)
                    .Fields(f => f
                        .Field(d => d.Name)
                        .Field(d => d.Author)
                        )
                    )
                ), cancellationToken);
        }

        public Task<ISearchResponse<ElasticsearchChapter>> SearchChapterAsync(string query, int skip, int take, CancellationToken cancellationToken)
        {
            return client.SearchAsync<ElasticsearchChapter>(document => document
                // Query this Index:
                .Index(indexChapter)
                // Paginate
                .From(skip)
                .Size(take)
                // Highlight Text Content:
                .Highlight(highlight => highlight
                    .Fields(fields => fields
                        .Fragmenter(HighlighterFragmenter.Span)
                        .PreTags("<font color='red'>")
                        .PostTags("</font>")
                        .FragmentSize(150)
                        .NoMatchSize(150)
                        .NumberOfFragments(1)
                        .Field(x => x.Content)
                    )
                )
                // Now kick off the query:
                .Query(q => q.MatchPhrase(m => m
                    .Query(query)
                    .Field(d => d.Content)
                    .Slop(3)
                    )
                ), cancellationToken);

                //.Query(q => q.MultiMatch(mm => mm
                //    .Query(query)
                //    .Type(TextQueryType.Phrase)
                //    .Fields(f => f
                //        .Field(d => d.Content)
                //        .Field(d => d.Name)))), cancellationToken);
        }
    }
}
