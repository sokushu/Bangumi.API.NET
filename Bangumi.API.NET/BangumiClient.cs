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
using Bangumi.API.NET.Exceptions;
using Bangumi.API.NET.Requests.Abstractions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static Bangumi.API.NET.IBangumiClient;

namespace Bangumi.API.NET
{
    public class BangumiClient : IBangumiClient
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        private HttpClient HttpClient { get; }

        private BangumiAPIOptions? Options { get; }

        private readonly CancellationTokenSource CancellationTokenSource;

        public event AsyncEventHandler<HttpRequestMessage>? SendingRequest;
        public event AsyncEventHandler<HttpResponseMessage>? ReceivedResponse;

        public BangumiClient(BangumiAPIOptions? options = null)
        {
            Options = options;

            var httpclientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
            };

            HttpClient = new HttpClient(httpclientHandler)
            {
                Timeout = Timeout,
            };

            string baseAddress;
            string userAgent;
            if (!string.IsNullOrEmpty(baseAddress = Options?.BaseURL ?? string.Empty))
                HttpClient.BaseAddress = new Uri(baseAddress);
            if (!string.IsNullOrEmpty(userAgent = Options?.UserAgent ?? string.Empty))
                HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            CancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            cancellationToken.ThrowIfCancellationRequested();

            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CancellationTokenSource.Token);

            var url = request.GetRequestURL();
            var requestMessage = request.ToHttpContent();

            var httpRequestMessage = new HttpRequestMessage(request.HttpMethod, url)
            {
                Content = requestMessage
            };

            if (SendingRequest != null)
                await SendingRequest.Invoke(this, httpRequestMessage);

            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await HttpClient.SendAsync(httpRequestMessage, tokenSource.Token);

                if (ReceivedResponse != null)
                    await ReceivedResponse.Invoke(this, responseMessage);

                if (!responseMessage.IsSuccessStatusCode && responseMessage.StatusCode >= HttpStatusCode.BadRequest)
                {
                    var errorJson = await responseMessage.Content.ReadAsStringAsync();
                    throw responseMessage.StatusCode switch
                    {
                        HttpStatusCode.BadRequest => new BangumiBadRequestException(errorJson),
                        HttpStatusCode.Unauthorized => new BangumiUnauthorizedException(errorJson),
                        HttpStatusCode.InternalServerError => new BangumiInternalServerErrorException(errorJson),
                        HttpStatusCode.NotFound => new BangumiNotFoundException(errorJson),
                        _ => new Exception($"Request to {url} failed with status code {responseMessage.StatusCode}: {errorJson}"),
                    };
                }

                var jsonObject = await request.ParseResponse(responseMessage);

                return jsonObject == null ? throw new Exception() : jsonObject;
            }
            catch (TaskCanceledException)
            {
                if (tokenSource.IsCancellationRequested)
                    throw;
                throw new TimeoutException($"Request to {url} timed out after {Timeout.TotalSeconds} seconds.");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while sending the request to {url}.", ex);
            }
            finally
            {
                requestMessage?.Dispose();
                httpRequestMessage.Dispose();
            }
        }
    }
}
