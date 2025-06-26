//MIT License

//Copyright (c) 2025 Sokushu

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.API.NET.Requests.Abstractions
{
    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
        public RequestBase(string methodname, HttpMethod? httpMethod = null)
        {
            MethodName = methodname;
            if (httpMethod != null)
                HttpMethod = httpMethod;
        }

        [JsonIgnore]
        public HttpMethod HttpMethod { get; } = HttpMethod.Get;

        [JsonIgnore]
        public string MethodName { get; }

        public virtual string GetRequestURL()
        {
            var sb = new StringBuilder(MethodName);
            var dictionary = new QueryDictionary();
            MakeRequestQuery(dictionary);
            var list = dictionary.ToList();
            if (list.Count > 0)
            {
                sb.Append("?");
                sb.Append(string.Join("&", list.Select(x => $"{x.Key}={x.Value}")));
            }

            return sb.ToString();
        }

        public virtual void MakeRequestQuery(QueryDictionary query) { }

        public virtual HttpContent? ToHttpContent()
        {
            var requestJson = JsonConvert.SerializeObject(this, GetType(), settings: new JsonSerializerSettings
            {

            });
            return new StringContent(requestJson);
        }

        public virtual async Task<TResponse> ParseResponse(HttpResponseMessage httpResponseMessage)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<TResponse>(responseString);
            return response!;
        }
    }
}
