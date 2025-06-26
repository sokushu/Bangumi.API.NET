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
using Bangumi.API.NET.Requests.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bangumi.API.NET.Requests
{
    /// <summary>
    /// Represents a request to execute a function with customizable query and response handling.
    /// </summary>
    /// <remarks>This class allows callers to define custom logic for constructing the query parameters and
    /// parsing the HTTP response. It is designed to be used in scenarios where the default behavior of <see
    /// cref="RequestBase{TResult}"/> needs to be extended or overridden.</remarks>
    /// <typeparam name="TResult">The type of the result returned by the function request.</typeparam>
    internal class FunctionRequest<TResult> : RequestBase<TResult>
    {
        /// <summary>
        /// Gets or sets the action to modify the request query parameters.
        /// </summary>
        /// <remarks>This action allows customization of the query parameters for a request by modifying
        /// the provided dictionary. The dictionary contains key-value pairs representing the query parameters, where
        /// the key is the parameter name and the value is its corresponding value.</remarks>
        public Action<QueryDictionary>? MakeRequestQueryAction { get; set; }

        /// <summary>
        /// Gets or sets the delegate that processes an <see cref="HttpResponseMessage"/>  and asynchronously returns a
        /// result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <remarks>Use this property to define custom logic for parsing HTTP responses.  The delegate
        /// should handle the response appropriately, such as checking  for errors or extracting data, and return the
        /// desired result.</remarks>
        public Func<HttpResponseMessage, Task<TResult>> ParseResponseAction { get; set; }

        /// <summary>
        /// Gets or sets the function used to convert data into an <see cref="HttpContent"/> instance.
        /// </summary>
        /// <remarks>This property allows customization of how data is transformed into HTTP content for
        /// use in HTTP requests. The function should return a valid <see cref="HttpContent"/> object or <see
        /// langword="null"/> if no content is required.</remarks>
        public Func<HttpContent?> ToHttpContentFunc { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionRequest"/> class with the specified method name and
        /// optional HTTP method.
        /// </summary>
        /// <remarks>This constructor sets up the request with default behaviors for parsing the response
        /// and generating HTTP content. Use this class to represent a function request with customizable HTTP method
        /// and response handling.</remarks>
        /// <param name="methodname">The name of the method to be invoked. This value cannot be null or empty.</param>
        /// <param name="httpMethod">The HTTP method to be used for the request. If not specified, a default value may be used.</param>
        public FunctionRequest(string methodname, HttpMethod? httpMethod = null) : base(methodname, httpMethod)
        {
            ParseResponseAction = (httpResponseMessage) =>
            {
                return base.ParseResponse(httpResponseMessage);
            };

            ToHttpContentFunc = () =>
            {
                return base.ToHttpContent();
            };
        }

        /// <summary>
        /// Parses the HTTP response message and returns the result of the specified type.
        /// </summary>
        /// <remarks>This method uses the <see cref="ParseResponseAction"/> delegate to process the
        /// response. Ensure that <see cref="ParseResponseAction"/> is properly configured to handle the expected
        /// response format.</remarks>
        /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the parsed response of type
        /// <typeparamref name="TResult"/>.</returns>
        public override async Task<TResult> ParseResponse(HttpResponseMessage httpResponseMessage)
        {
            return await ParseResponseAction(httpResponseMessage);
        }

        /// <summary>
        /// Modifies the provided query dictionary to prepare it for a request.
        /// </summary>
        /// <remarks>This method invokes the <see cref="MakeRequestQueryAction"/> delegate, if set, to
        /// modify the query dictionary. Ensure that the dictionary is not null before calling this method.</remarks>
        /// <param name="query">A dictionary containing query parameters to be modified. Keys represent parameter names, and values
        /// represent parameter values.</param>
        public override void MakeRequestQuery(QueryDictionary query)
        {
            MakeRequestQueryAction?.Invoke(query);
        }

        /// <summary>
        /// Converts the current instance into an <see cref="HttpContent"/> object.
        /// </summary>
        /// <remarks>This method invokes the delegate specified by <see cref="ToHttpContentFunc"/> to
        /// generate the <see cref="HttpContent"/>. If <see cref="ToHttpContentFunc"/> is null, the method returns
        /// null.</remarks>
        /// <returns>An <see cref="HttpContent"/> object representing the current instance, or <see langword="null"/> if <see
        /// cref="ToHttpContentFunc"/> is not set.</returns>
        public override HttpContent? ToHttpContent()
        {
            return ToHttpContentFunc?.Invoke();
        }
    }
}
