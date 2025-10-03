namespace ComuunityHub.Interfaces.Services;

public interface IElasticService
{
    Task IndexAsync<T>(T document, string indexName) where T : class;
    Task<List<T>> SearchAsync<T>(string keyword, string indexName, params string[] fields) where T : class;
}