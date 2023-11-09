using System.Net.Http.Json;

namespace MotoGP
{
    public class MotoGpClient : HttpClient
    {
        private readonly HttpClient client;

        public MotoGpClient(HttpClient client)
        {
            this.client = client;
        }

        public Task<T?> GetAsync<T>(Uri relativeUrl, CancellationToken token = default)
        {
            return client.GetFromJsonAsync<T>(relativeUrl, token);
        }
    }
}