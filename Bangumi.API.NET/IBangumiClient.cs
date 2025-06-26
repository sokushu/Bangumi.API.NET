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
using System.Threading.Tasks;
using System.Threading;
using Bangumi.API.NET.Requests.Abstractions;
using System.Net.Http;

namespace Bangumi.API.NET
{
    /// <summary>
    /// Defines the contract for a client that interacts with the Bangumi API.
    /// </summary>
    /// <remarks>This interface provides methods for sending requests to the Bangumi API and managing access
    /// tokens. It also includes events for monitoring HTTP request and response activity, as well as a configurable
    /// timeout. Implementations of this interface should handle API-specific behaviors, such as authentication and
    /// request formatting.</remarks>
    public interface IBangumiClient : IDisposable
    {
        /// <summary>
        /// Represents a method that handles an asynchronous event with event data of type <typeparamref
        /// name="TEventArgs"/>.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data passed to the handler.</typeparam>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An object containing the event data.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);

        /// <summary>
        /// Occurs before an HTTP request is sent, allowing subscribers to inspect or modify the request.
        /// </summary>
        /// <remarks>This event is triggered asynchronously before the HTTP request is dispatched. 
        /// Subscribers can use this event to modify headers, adjust the request content, or perform other 
        /// pre-processing tasks. Ensure that any modifications to the <see cref="HttpRequestMessage"/>  are thread-safe
        /// and do not introduce side effects.</remarks>
        public event AsyncEventHandler<HttpRequestMessage>? SendingRequest;

        /// <summary>
        /// Occurs when an HTTP response is received.
        /// </summary>
        /// <remarks>This event is triggered after an HTTP response is successfully received.  Subscribers
        /// can use this event to process the response or perform additional actions. The event handler is asynchronous,
        /// allowing for non-blocking operations.</remarks>
        public event AsyncEventHandler<HttpResponseMessage>? ReceivedResponse;

        /// <summary>
        /// Gets or sets the timeout duration for the operation.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Retrieves an access token for the Bangumi API using the specified options.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to obtain an access token for
        /// authenticating with the Bangumi API. Ensure that the provided options include valid client credentials and
        /// any required parameters.</remarks>
        /// <param name="bangumiAPIAccessTokenOptions">The options used to configure the request for the access token, including client credentials and other
        /// parameters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access token.</returns>
        public Task GetAccessToken(BangumiAPIAccessTokenOptions bangumiAPIAccessTokenOptions);

        /// <summary>
        /// Sends a request to the server and asynchronously retrieves the response.
        /// </summary>
        /// <remarks>This method sends the specified request to the server and waits for the response. The
        /// caller must ensure that the request object is properly configured and compatible with the server's
        /// API.</remarks>
        /// <typeparam name="TResponse">The type of the response expected from the server.</typeparam>
        /// <param name="request">The request to be sent. Must implement <see cref="IRequest{TResponse}"/>.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task representing the asynchronous operation. The result contains the response of type <typeparamref
        /// name="TResponse"/>.</returns>
        public Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
