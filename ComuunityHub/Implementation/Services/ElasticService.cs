using ComuunityHub.Interfaces.Services;
using Elastic.Clients.Elasticsearch;

namespace ComuunityHub.Implementation.Services;

public class ElasticService : IElasticService
{
    private readonly ElasticsearchClient _elasticClient;

    public ElasticService(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    public async Task IndexAsync<T>(T document, string indexName) where T : class
    {
        await _elasticClient.IndexAsync(document, i => i.Index(indexName));
    }
    public async Task<List<T>> SearchAsync<T>(string keyword, string indexName, params string[] fields) where T : class
    {
        var response = await _elasticClient.SearchAsync<T>(s => s
            .Indices(indexName)
            .Query(q => q
                .MultiMatch(m => m
                        .Fields(fields)
                        .Query(keyword)
                        .Fuzziness(new Elastic.Clients.Elasticsearch.Fuzziness("AUTO"))
                )
            )
        );

        return response.IsValidResponse ? response.Documents.ToList() : new List<T>();
    }
}