using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Launcher.Core
{
    public class WebClient : IWebClient
    {
        public HttpClient Client { get => webClientAsync.Client; }

        readonly WebClientAsync webClientAsync;

        public WebClient()
        {
            webClientAsync = new WebClientAsync();
        }

        public void Get(string URL, Action<HttpResponseMessage> response, Action<Exception> error)
        {
            var task = webClientAsync.GetAsync(URL, error);

            Task sub = new Task(() => task.Start());
            sub.RunSynchronously();

            if (task.Result != null)
            {
                response?.Invoke(task.Result);
            }
        }

        public void Post(string URL, Dictionary<string, object> fields, Action<HttpResponseMessage> response, Action<Exception> error)
        {
            var task = webClientAsync.PostAsync(URL, fields, error);

            Task sub = new Task(() => task.Start());
            sub.RunSynchronously();

            if (task.Result != null)
            {
                response?.Invoke(task.Result);
            }
        }

        //

        public static void HTTPError(HttpStatusCode code)
        {
            Console.WriteLine($"{(int)code}/{code}");
        }

        public static void ProcessResponse(HttpWebResponse response, Action<string> onSuccess, Action<HttpStatusCode> onFailure = null)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            onFailure = onFailure ?? HTTPError;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    onSuccess?.Invoke(data);
                }
            }
            else
            {
                onFailure?.Invoke(response.StatusCode);
            }
        }

        public static async void ProcessResponse(HttpResponseMessage response, Action<string> onSuccess, Action<HttpStatusCode> onFailure = null)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            onFailure = onFailure ?? HTTPError;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string data = await response.Content.ReadAsStringAsync();
                onSuccess?.Invoke(data);
            }
            else
            {
                onFailure?.Invoke(response.StatusCode);
            }
        }

        public static string BuildQuery(Dictionary<string, object> fields)
        {
            var pairs = fields.Select(pair => $"{pair.Key}={pair.Value}");
            return string.Join("&", pairs);
        }
    }
}