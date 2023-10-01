using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;

namespace Launcher.Core
{
    public interface IWebClient
    {
        void Get(string URL, Action<HttpResponseMessage> response, Action<Exception> error);
        void Post(string URL, Dictionary<string, object> fields, Action<HttpResponseMessage> response, Action<Exception> error);
    }
}
