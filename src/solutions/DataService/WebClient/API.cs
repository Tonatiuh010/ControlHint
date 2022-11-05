using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http.Headers;
using DataService.Interfaces;

namespace DataService.WebClient
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

        public async Task<HttpResponseMessage> GetRequest(RequestProperties props) => await PrepareRequest(props.SetGetMethod());

        public async Task<HttpResponseMessage> PostRequest(RequestProperties props) => await PrepareRequest(props.SetPostMethod());

        private async Task<HttpResponseMessage> PrepareRequest(RequestProperties props)
        {
            HttpRequestHeaders defaultHeaders = Headers;

            if (props.Headers != null)
                AddHeaders(defaultHeaders, props.Headers);

            return await MakeRequest(props);
        }

        private async Task<HttpResponseMessage> MakeRequest(RequestProperties props)
        {
            HttpRequestMessage message = new(props.Method, props.EndPoint);

            if (props.Params != null)            
                message.Content = props.GetContent();            

            if (props.Headers != null)            
                AddHeaders(message.Headers, props.Headers);            

            return await Client.SendAsync(message);            
        }

        public void AddHeaders(HttpHeaders extraHeaders) => AddHeaders(this.Headers, extraHeaders);

        public void Dispose() => GC.SuppressFinalize(this);


        #region static methods
        public static void SetClient(WebRequest request)
        {
            var client = request.Client;
            var url = request.Url;
            if (client != null)
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();                

                foreach(var header in GetValidHeaders())
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
            foreach(var h in extraHeaders)
            {
                inValue.Add(h.Key, h.Value);
            }
        }
        #endregion
        
    }

    public class RequestProperties
    {
        private HttpContent? _content { get; set; }

        public string EndPoint { get; set; }
        public HttpMethod Method { get; set; }
        public IHttpContentConverter? Params { get; set; }
        public HttpHeaders? Headers { get; set; }        


        public RequestProperties()
        {
            EndPoint = string.Empty;            
            Method = HttpMethod.Get;
            Params = null;
            Headers = null;
        }

        public RequestProperties(string url, HttpMethod method, IHttpContentConverter? @params = null, HttpHeaders? headers = null)
        {
            EndPoint = url;            
            Method = method;
            Params = @params;
            Headers = headers;
        }

        public HttpContent? GetContent() => _content; // Params?.GetContent();

        public RequestProperties SetGetMethod()
        {
            Method = HttpMethod.Get;
            return this;
        }

        public RequestProperties SetPostMethod()
        {
            Method = HttpMethod.Post;
            return this;
        }

        public void SetContent(IHttpContentConverter contentConverter)
        {
            _content = contentConverter.GetContent();
        }

        public static HttpMethod GetMethod(string method)
        {
            switch (method) {
                case "GET": return HttpMethod.Get;
                case "POST": return HttpMethod.Post;
                case "PUT": return HttpMethod.Put;
                case "DELETE": return HttpMethod.Delete;                
                default: return HttpMethod.Get;
            }
        }

    }

}
