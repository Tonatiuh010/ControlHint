using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http.Headers;

namespace WebRequest
{
    public class WebRequest : IDisposable
    {
        private HttpClient Client { get; set; }
        public string Url { get; set; }
        public HttpRequestHeaders Headers { get; set; }

        public WebRequest(string url)
        {
            Client = new();
            Url = url;
            Headers = Client.DefaultRequestHeaders;

            SetClient(this);
        }

        public async Task<HttpResponseMessage> GetRequest(
            string extension,
            List<KeyValuePair<string, string>>? data = null,
            HttpRequestHeaders? extraHeaders = null
        )
        {
            HttpRequestHeaders defaultHeaders = Headers;

            if (extraHeaders != null)
            {
                AddHeaders(defaultHeaders, extraHeaders);
            }


            return await MakeRequest(
                extension,
                HttpMethod.Get,
                defaultHeaders,
                data != null ? () => new FormUrlEncodedContent(data) : null
            );
        }

        public async Task<HttpResponseMessage> PostRequest(
            string extension,
            List<KeyValuePair<string, string>>? data = null,
            HttpRequestHeaders? extraHeaders = null
        )
        {
            HttpRequestHeaders defaultHeaders = Headers;

            if (extraHeaders != null)
            {
                AddHeaders(defaultHeaders, extraHeaders);
            }


            return await MakeRequest(
                extension,
                HttpMethod.Post,
                defaultHeaders,
                data != null ? () => new FormUrlEncodedContent(data) : null
            );
        }

        protected async Task<HttpResponseMessage> MakeRequest(
            string extension,
            HttpMethod method,
            HttpRequestHeaders headers,
            Func<HttpContent>? getContent = null
        )
        {
            HttpRequestMessage message = new(method, extension);

            if (getContent != null)
            {
                message.Content = getContent();
            }

            if (headers != null && headers.Count() > 0)
            {
                AddHeaders(message.Headers, headers);
            }

            return await Client.SendAsync(message);
        }

        public void AddHeaders(HttpHeaders extraHeaders) => AddHeaders(this.Headers, extraHeaders);

        public void Dispose() => GC.SuppressFinalize(this);

        public static void SetClient(WebRequest request)
        {
            var client = request.Client;
            var url = request.Url;
            if (client != null)
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();

                foreach (var header in GetValidHeaders())
                    client.DefaultRequestHeaders.Accept.Add(header);
            }
        }

        private static List<MediaTypeWithQualityHeaderValue> GetValidHeaders() =>
            new() {
                new ("application/json"),
                new ("application/text"),
                new ("application/html"),
            };

        private static void AddHeaders(HttpHeaders inValue, HttpHeaders extraHeaders)
        {
            foreach (var h in extraHeaders)
            {
                inValue.Add(h.Key, h.Value);
            }
        }
    }
}
