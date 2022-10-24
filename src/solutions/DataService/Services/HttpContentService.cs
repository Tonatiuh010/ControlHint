using DataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Services
{
    public class HttpContentService : IHttpContentConverter
    {
        public object Params { get; set; }

        public HttpContentService(object @params) => Params = @params;

        public HttpContent GetContent()
        {
            switch (Params)
            {
                case byte[] bytes: 
                    return new ByteArrayContent(bytes);

                case string str: 
                    return new StringContent(str);

                case List<KeyValuePair<string, string>> @params: 
                    return new FormUrlEncodedContent(@params);

                case ReadOnlyMemory<byte> memoryList:
                    return new ReadOnlyMemoryContent(memoryList);

                case Stream stream:
                    return new StreamContent(stream);

                case List<HttpContent> contents:
                    var multipart = new MultipartContent();

                    foreach (var content in contents)
                        multipart.Add(content);

                    return multipart;

                default:
                    return JsonContent.Create(Params, Params.GetType());
            }

            //case "MultipartFormDataContent":
            //    var multipartForm = new MultipartFormDataContent();
            //    var contents1 = (List<HttpContent>)Params;
            //    foreach (var content in contents1)
            //        multipartForm.Add(content);
            //    return multipartForm;

        }
    }
}
