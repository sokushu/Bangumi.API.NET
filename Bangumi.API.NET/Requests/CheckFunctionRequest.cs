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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bangumi.API.NET.Requests
{
    /// <summary>
    /// Represents a request to check the status of a function, with support for validating HTTP status codes.
    /// </summary>
    /// <remarks>This class is used to perform a function request and determine whether the response meets
    /// specific criteria. The validation is based on the HTTP status code of the response, which can be customized
    /// using the  <see cref="HttpStatusCodes"/> property.</remarks>
    internal class CheckFunctionRequest : FunctionRequest<bool>
    {
        /// <summary>
        /// Gets or sets the collection of HTTP status codes that are relevant for the operation.
        /// </summary>
        public HashSet<HttpStatusCode>? HttpStatusCodes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckFunctionRequest"/> class with the specified method name
        /// and optional HTTP method.
        /// </summary>
        /// <remarks>This class is designed to handle requests and validate HTTP response status codes.
        /// The <see cref="ParseResponseAction"/> delegate is preconfigured to check whether the response status code
        /// matches the expected values defined in <see cref="HttpStatusCodes"/> or defaults to <see
        /// cref="HttpStatusCode.OK"/> if no status codes are specified.</remarks>
        /// <param name="methodname">The name of the method to be invoked. This parameter cannot be null or empty.</param>
        /// <param name="httpMethod">The HTTP method to be used for the request. If not specified, the default HTTP method will be used.</param>
        public CheckFunctionRequest(string methodname, HttpMethod? httpMethod = null) : base(methodname, httpMethod)
        {
            ParseResponseAction = async (httpResponseMessage) =>
            {
                if (HttpStatusCodes == null)
                    return httpResponseMessage.StatusCode == HttpStatusCode.OK;
                return await Task.FromResult(HttpStatusCodes.Contains(httpResponseMessage.StatusCode));
            };
        }
    }
}
