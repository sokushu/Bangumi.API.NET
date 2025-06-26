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
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Bangumi.API.NET.Requests
{
    /// <summary>
    /// Represents a request to retrieve photo data using a specified function.
    /// </summary>
    /// <remarks>This class is used to configure and execute a function request that retrieves photo data. The
    /// response is parsed based on the HTTP status code: <list type="bullet"> <item> <description>If the status code is
    /// <see cref="HttpStatusCode.OK"/>, the response content is read as a string using the specified <see
    /// cref="Encoding"/>.</description> </item> <item> <description>If the status code indicates a redirect (<see
    /// cref="HttpStatusCode.Moved"/>, <see cref="HttpStatusCode.MovedPermanently"/>, <see
    /// cref="HttpStatusCode.Found"/>, or <see cref="HttpStatusCode.Redirect"/>), the redirect URL is
    /// returned.</description> </item> <item> <description>For other status codes, an exception is thrown containing
    /// the method name and the status code.</description> </item> </list></remarks>
    internal class GetPhotoFunctionRequest : FunctionRequest<string>
    {
        /// <summary>
        /// Gets or sets the character encoding used for text processing.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPhotoFunctionRequest"/> class, specifying the method name
        /// and optional HTTP method.
        /// </summary>
        /// <remarks>This constructor sets up the request and defines a response parsing action. The
        /// response parsing action handles HTTP responses with status codes indicating success, redirection, or
        /// failure: <list type="bullet"> <item> <description> For <see cref="HttpStatusCode.OK"/>, the response content
        /// is read as a string. </description> </item> <item> <description> For redirection status codes (<see
        /// cref="HttpStatusCode.Moved"/>, <see cref="HttpStatusCode.MovedPermanently"/>, <see
        /// cref="HttpStatusCode.Found"/>, or <see cref="HttpStatusCode.Redirect"/>), the URL from the response's <see
        /// cref="System.Net.Http.Headers.HttpResponseHeaders.Location"/> is returned. </description> </item> <item>
        /// <description> For other status codes, an exception is thrown containing the method name and the status code.
        /// </description> </item> </list></remarks>
        /// <param name="methodname">The name of the method to be invoked. This value cannot be null or empty.</param>
        /// <param name="httpMethod">The HTTP method to be used for the request. If not specified, the default HTTP method will be used.</param>
        /// <exception cref="Exception">Thrown if the HTTP response status code is not <see cref="HttpStatusCode.OK"/> or a redirection status code.
        /// The exception message includes the method name and the status code.</exception>
        public GetPhotoFunctionRequest(string methodname, HttpMethod? httpMethod = null) : base(methodname, httpMethod)
        {
            ParseResponseAction = async (httpResponseMessage) =>
            {
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync(), Encoding);
                    return await streamReader.ReadToEndAsync();
                }
                else if (httpResponseMessage.StatusCode == HttpStatusCode.Moved || httpResponseMessage.StatusCode == HttpStatusCode.MovedPermanently
                    || httpResponseMessage.StatusCode == HttpStatusCode.Found || httpResponseMessage.StatusCode == HttpStatusCode.Redirect)
                {
                    var url = httpResponseMessage.Headers.Location.ToString();
                    return url;
                }
                throw new Exception(MethodName + " failed: " + httpResponseMessage.StatusCode.ToString());
            };
        }
    }
}
