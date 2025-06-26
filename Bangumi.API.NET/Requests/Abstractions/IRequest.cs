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
using System.Net.Http;
using System.Threading.Tasks;

namespace Bangumi.API.NET.Requests.Abstractions
{
    /// <summary>
    /// Represents a contract for building and managing HTTP requests in an API context.
    /// </summary>
    /// <remarks>The <see cref="IRequest"/> interface provides a standardized way to define and interact with
    /// HTTP requests, including specifying the HTTP method, API version, and constructing the request URL. It also
    /// supports serialization of the request data into an <see cref="HttpContent"/> format for use with HTTP client
    /// operations. Implementations of this interface are expected to encapsulate the details of request construction
    /// and serialization, making it easier to work with APIs in a consistent manner.</remarks>
    public interface IRequest
    {
        /// <summary>
        /// Gets the HTTP method associated with the request.
        /// </summary>
        public HttpMethod HttpMethod { get; }

        /// <summary>
        /// Gets the name of the method associated with the current operation.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Constructs and returns the full URL for the current request.
        /// </summary>
        /// <remarks>This method is typically used to retrieve the complete URL for the current HTTP
        /// request. Ensure that the application is running in a context where a request URL can be generated.</remarks>
        /// <returns>A string representing the full URL of the current request, including the protocol, host, and path.</returns>
        public string GetRequestURL();

        /// <summary>
        /// Converts the current object into an <see cref="HttpContent"/> instance.
        /// </summary>
        /// <remarks>Use this method to serialize the object into a format suitable for HTTP requests,
        /// such as JSON or form data. The returned <see cref="HttpContent"/> can be used with HTTP client methods like
        /// <see cref="HttpClient.PostAsync(string, HttpContent)"/>.</remarks>
        /// <returns>An <see cref="HttpContent"/> representation of the current object, or <see langword="null"/> if the
        /// conversion is not possible.</returns>
        public HttpContent? ToHttpContent();
    }

    /// <summary>
    /// Represents a request that produces a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <remarks>This interface is intended for requests that return a specific response type. Implementations
    /// should define how the response is parsed from an <see cref="HttpResponseMessage"/>.</remarks>
    /// <typeparam name="TResponse">The type of the response produced by the request.</typeparam>
    public interface IRequest<TResponse> : IRequest
    {
        public Task<TResponse> ParseResponse(HttpResponseMessage httpResponseMessage);
    }
}
