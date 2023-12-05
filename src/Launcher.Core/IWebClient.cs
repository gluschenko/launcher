using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Launcher.Core
{
    public interface IWebClient
    {
        void Get(string URL, Action<HttpResponseMessage> response, Action<Exception> error);
        void Post(string URL, Dictionary<string, object> fields, Action<HttpResponseMessage> response, Action<Exception> error);
    }
}
