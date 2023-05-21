using OqTepaLavash.Services.IService;
using System.Net.Http.Headers;
using System;
using Models.LoginModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace OqTepaLavash.Services
{
    public class HttpRequestsService : IHttpRequestsService
    {
        private readonly string url;
        private string? token;        
        
        public HttpRequestsService(IOptions<Config> url)
        {            
            if (url.Value != null && !string.IsNullOrEmpty(url.Value.url))
                this.url = url.Value.url;
            else
                this.url = "";
        }

        public HttpClient getClient(bool withToken = true)
        {
            HttpClientHandler clientHandler = new();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            //HttpClient client = new(clientHandler);
            HttpClient client = new();
            client.Timeout = TimeSpan.FromMinutes(1);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            if (withToken)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<HttpResponseMessage> GetData(string query)
        {
            HttpClient client = getClient();
            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(query);
                client.Dispose();
                string content = await responseMessage.Content.ReadAsStringAsync();
                var responseStatus = responseMessage.StatusCode;
                switch (responseStatus)
                {
                    case System.Net.HttpStatusCode.OK:
                        return responseMessage;
                    default:
                        throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<HttpResponseMessage> PostData<T>(string query, T model)
        {
            HttpClient client = getClient();
            var param = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(query, param);
            client.Dispose();
            string content = await responseMessage.Content.ReadAsStringAsync();
            
            var responseStatus = responseMessage.StatusCode;
            switch (responseStatus)
            {
                case System.Net.HttpStatusCode.OK:
                    return responseMessage;
                case System.Net.HttpStatusCode.Created:
                    return responseMessage;                                    
                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception(content);                                        
                case System.Net.HttpStatusCode.NotFound:
                    throw new Exception(content);
                case System.Net.HttpStatusCode.BadRequest:                    
                    throw new Exception(content);
                default:
                    throw new Exception(content);

            }
        }
    }
}
