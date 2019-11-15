using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Core
{
    public class WebClientAsync : IWebClient
    {
        public HttpClient Client { get; private set; }

        public WebClientAsync()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("User-Agent", "123");
        }

        ~WebClientAsync()
        {
            Client.Dispose();
        }

        public async void Get(string URL, Action<HttpResponseMessage> response, Action<Exception> error)
        {
            var res = await GetAsync(URL, error);
            response?.Invoke(res);
        }

        public async void Post(string URL, Dictionary<string, object> fields, Action<HttpResponseMessage> response, Action<Exception> error)
        {
            var res = await PostAsync(URL, fields, error);
            response?.Invoke(res);
        }

        //

        public Task<HttpResponseMessage> GetAsync(string URL, Action<Exception> error)
        {
            try
            {
                return Client.GetAsync(URL);
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
            }

            return null;
        }


        public Task<HttpResponseMessage> PostAsync(string URL, Dictionary<string, object> fields, Action<Exception> error)
        {
            using (var message = new HttpRequestMessage(HttpMethod.Post, URL)) 
            {
                var query = WebClient.BuildQuery(fields);
                message.Content = new StringContent(query, Encoding.UTF8, "application/x-www-form-urlencoded");

                try
                {
                    return Client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    error?.Invoke(ex);
                }
            }
            return null;
        }
    }
}
