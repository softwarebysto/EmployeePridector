namespace OqTepaLavash.Services.IService
{
    public interface IHttpRequestsService
    {
        Task<HttpResponseMessage> GetData(string query);
        Task<HttpResponseMessage> PostData<T>(string query, T model);
        
    }
}
