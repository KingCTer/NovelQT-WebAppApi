using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NovelQT.Application.Elasticsearch;

namespace NovelQT.Application.Elasticsearch.Hosting
{
    public class ElasticsearchInitializerHostedService : IHostedService
    {
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly ILogger<ElasticsearchInitializerHostedService> logger;

        public ElasticsearchInitializerHostedService(
            ILogger<ElasticsearchInitializerHostedService> logger, 
            ElasticsearchClient elasticsearchClient)
        {
            this.logger = logger;
            this.elasticsearchClient = elasticsearchClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Now we can wait for the Shards to boot up
            var healthTimeout = TimeSpan.FromSeconds(50);

            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Waiting for at least 1 Node and at least 1 Active Shard, with a Timeout of {healthTimeout.TotalSeconds} seconds.");
            }

            await elasticsearchClient.WaitForClusterAsync(healthTimeout, cancellationToken);

            // Prepare Elasticsearch Database:
            var indexExistsResponse = await elasticsearchClient.ExistsBookAsync(cancellationToken);

            if (!indexExistsResponse.Exists)
            {
                await elasticsearchClient.CreateBookIndexAsync(cancellationToken);
                //await elasticsearchClient.CreatePipelineAsync(cancellationToken);
            }

            // Prepare Elasticsearch Database:
            indexExistsResponse = await elasticsearchClient.ExistsChapterAsync(cancellationToken);

            if (!indexExistsResponse.Exists)
            {
                await elasticsearchClient.CreateChapterIndexAsync(cancellationToken);
                //await elasticsearchClient.CreatePipelineAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
