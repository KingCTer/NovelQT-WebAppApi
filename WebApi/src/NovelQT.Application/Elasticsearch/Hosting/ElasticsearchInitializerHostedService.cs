using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using NovelQT.Application.Elasticsearch;

namespace NovelQT.Application.Elasticsearch.Hosting
{
    public class ElasticsearchInitializerHostedService : IHostedService
    {
        private readonly ElasticsearchClient elasticsearchClient;
        private readonly ILogger<ElasticsearchInitializerHostedService> logger;
        private readonly ElasticsearchOptions elasticsearchOptions;

        public ElasticsearchInitializerHostedService(
            ILogger<ElasticsearchInitializerHostedService> logger, 
            ElasticsearchClient elasticsearchClient,
            IOptions<ElasticsearchOptions> options
            )
        {
            this.logger = logger;
            this.elasticsearchClient = elasticsearchClient;
            this.elasticsearchOptions = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!elasticsearchOptions.IsUseElasticsearch)
            {
                if (logger.IsDebugEnabled())
                {
                    logger.LogDebug($"Elasticsearch is disable.");
                }
                return;
            }

            // Now we can wait for the Shards to boot up
            var healthTimeout = TimeSpan.FromSeconds(50);
            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Elasticsearch is enable, Elastic Cloud is {elasticsearchOptions.IsUseCloud}");
                logger.LogDebug($"Check health with Timeout of {healthTimeout.TotalSeconds} seconds...");
            }

            if (elasticsearchOptions.IsUseCloud)
            {
                ClusterHealthResponse healthResponse = await elasticsearchClient.WaitForClusterHealthAsync(healthTimeout, cancellationToken);
                if (logger.IsDebugEnabled())
                {
                    if (healthResponse.ApiCall.Success)
                    {
                        logger.LogDebug($"Connecting to Elastic Cloud is Success: {healthResponse.Status}");
                    }
                    else
                    {
                        logger.LogDebug($"Connecting to Elastic Cloud is failure: {healthResponse.DebugInformation}");
                        return;
                    }

                }
                if (healthResponse.ApiCall.Success == false) return;
                
            } else
            {
                ClusterHealthResponse healthResponse = await elasticsearchClient.WaitForClusterAsync(healthTimeout, cancellationToken);
                if (logger.IsDebugEnabled())
                {
                    if (healthResponse.ApiCall.Success)
                    {
                        logger.LogDebug($"Connecting to Elasticsearch Docker is Success: {healthResponse.Status}");
                    }
                    else
                    {
                        logger.LogDebug($"Connecting to Elasticsearch Docker is failure: {healthResponse.DebugInformation}");
                        return;
                    }

                }
                if (healthResponse.ApiCall.Success == false) return;
            }


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
