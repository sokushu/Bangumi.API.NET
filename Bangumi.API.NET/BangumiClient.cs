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
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static Bangumi.API.NET.IBangumiClient;

namespace Bangumi.API.NET
{
    /// <summary>
    /// Provides functionality for interacting with the Bangumi API, including sending requests, handling responses, and
    /// managing authentication.
    /// </summary>
    /// <remarks>The <see cref="BangumiClient"/> class is designed to facilitate communication with the
    /// Bangumi API. It supports customizable options such as timeout settings, base URL, and user agent configuration.
    /// Events are provided to allow handling of HTTP requests and responses, enabling advanced scenarios such as
    /// logging or modifying requests before they are sent.  This class is thread-safe for concurrent use, but care
    /// should be taken when modifying shared state such as event handlers or options.</remarks>
    public class BangumiClient : IBangumiClient
    {
        /// <summary>
        /// Gets or sets the timeout duration for HTTP requests.
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return HttpClientInstance.Timeout;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value), "Timeout must be greater than zero.");
                HttpClientInstance.Timeout = value;
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="HttpClient"/> instance used for making HTTP requests.
        /// </summary>
        private HttpClient HttpClientInstance { get; }

        /// <summary>
        /// Gets the instance of <see cref="HttpClientHandler"/> used to configure HTTP request handling.
        /// </summary>
        private HttpClientHandler HttpClientHandlerInstance { get; }

        /// <summary>
        /// Gets the current instance of <see cref="BangumiAPIOptions"/> used for configuring API behavior.
        /// </summary>
        private BangumiAPIOptions? OptionsInstance { get; }

        /// <summary>
        /// Represents a source for cancellation tokens that can be used to signal cancellation of asynchronous
        /// operations.
        /// </summary>
        /// <remarks>This field is intended for internal use to manage cancellation of tasks or
        /// operations. It provides a centralized mechanism to issue cancellation requests.</remarks>
        private readonly CancellationTokenSource _CancellationTokenSource;

        /// <summary>
        /// Occurs before an HTTP request is sent, allowing subscribers to inspect or modify the request.
        /// </summary>
        /// <remarks>This event is triggered asynchronously before the HTTP request is dispatched. 
        /// Subscribers can use this event to modify headers, adjust the request content, or log details. Ensure that
        /// any modifications to the <see cref="HttpRequestMessage"/> are thread-safe.</remarks>
        public event AsyncEventHandler<HttpRequestMessage>? SendingRequest;

        /// <summary>
        /// Occurs when an HTTP response is received.
        /// </summary>
        /// <remarks>This event is triggered after an HTTP request is completed and a response is
        /// received. Subscribers can use this event to process the response or perform additional actions. The event
        /// handler must be asynchronous and return a <see cref="Task"/>.</remarks>
        public event AsyncEventHandler<HttpResponseMessage>? ReceivedResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="BangumiClient"/> class, configured with the specified API
        /// options.
        /// </summary>
        /// <remarks>The <see cref="BangumiClient"/> class provides functionality to interact with the
        /// Bangumi API. It uses an <see cref="HttpClient"/> internally to handle HTTP requests and responses.  If
        /// <paramref name="options"/> is provided, the following settings are applied: <list type="bullet">
        /// <item><description><see cref="BangumiAPIOptions.AllowAutoRedirect"/> determines whether HTTP redirects are
        /// automatically followed.</description></item> <item><description><see cref="BangumiAPIOptions.UseCookie"/>
        /// specifies whether cookies are used for requests.</description></item> <item><description><see
        /// cref="BangumiAPIOptions.BaseURL"/> sets the base URL for API requests.</description></item>
        /// <item><description><see cref="BangumiAPIOptions.UserAgent"/> sets the User-Agent header for
        /// requests.</description></item> </list> If no options are provided, default values are used for these
        /// settings.</remarks>
        /// <param name="options">An optional <see cref="BangumiAPIOptions"/> object that specifies configuration settings for the client. If
        /// null, default settings are used.</param>
        public BangumiClient(BangumiAPIOptions? options = null)
        {
            OptionsInstance = options;

            HttpClientHandlerInstance = new HttpClientHandler()
            {
                AllowAutoRedirect = OptionsInstance?.AllowAutoRedirect ?? false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = OptionsInstance?.UseCookie ?? true,
                CookieContainer = new CookieContainer(),
            };

            HttpClientInstance = new HttpClient(HttpClientHandlerInstance)
            {
                Timeout = Timeout,
            };

            // HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Empty);

            string baseAddress;
            string userAgent;
            if (!string.IsNullOrEmpty(baseAddress = OptionsInstance?.BaseURL ?? string.Empty))
                HttpClientInstance.BaseAddress = new Uri(baseAddress);
            if (!string.IsNullOrEmpty(userAgent = OptionsInstance?.UserAgent ?? string.Empty))
                HttpClientInstance.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            _CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends an HTTP request based on the specified <see cref="IRequest{TResponse}"/> and returns the response.
        /// </summary>
        /// <remarks>This method sends an HTTP request using the provided <paramref name="request"/>
        /// object, which must implement <see cref="IRequest{TResponse}"/>. The method handles common HTTP errors by
        /// throwing specific exceptions for certain status codes. If the request is successful, the response is parsed
        /// using the <see cref="IRequest{TResponse}.ParseResponse"/> method.</remarks>
        /// <typeparam name="TResponse">The type of the response expected from the request.</typeparam>
        /// <param name="request">The request object containing the details of the HTTP request to be sent. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// provided.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the parsed response of type
        /// <typeparamref name="TResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="TimeoutException">Thrown if the request times out.</exception>
        /// <exception cref="Exception"></exception>
        public virtual async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            cancellationToken.ThrowIfCancellationRequested();

            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _CancellationTokenSource.Token);

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
                responseMessage = await HttpClientInstance.SendAsync(httpRequestMessage, tokenSource.Token);

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

        public async Task GetAccessToken(BangumiAPIAccessTokenOptions bangumiAPIAccessTokenOptions)
        {
            if (bangumiAPIAccessTokenOptions == null)
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions));
            if (string.IsNullOrEmpty(bangumiAPIAccessTokenOptions.Email))
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions.Email));
            if (string.IsNullOrEmpty(bangumiAPIAccessTokenOptions.Password))
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions.Password));
            if (string.IsNullOrEmpty(bangumiAPIAccessTokenOptions.ListenUrl))
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions.ListenUrl));
            if (string.IsNullOrEmpty(bangumiAPIAccessTokenOptions.ClientId))
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions.ClientId));
            if (string.IsNullOrEmpty(bangumiAPIAccessTokenOptions.ResponseType))
                throw new ArgumentNullException(nameof(bangumiAPIAccessTokenOptions.ResponseType));

            // GET https://bgm.tv/oauth/authorize
            //client_id   string App ID 注册应用时获取	☑️
            //response_type   string 验证类型    目前仅支持 code	☑️
            //redirect_uri    string 回调 URL 在后台设置的回调地址
            //scope   string 请求权限    尚未实现
            //state   string 随机参数    随机生产的参数，便于开发者防止跨站攻击

            //POST https://bgm.tv/oauth/access_token
            //Parameter Type    Desc Note    Required
            //grant_type  string 授权方式    此处应使用 authorization_code	☑️
            //client_id   string App ID 注册应用时获取	☑️
            //client_secret   string App Secret 注册应用时获取	☑️
            //code    string 验证代码    回调获取的 code	☑️
            //redirect_uri    string 回调 URL 在后台设置的回调地址	☑️
            //state   string 随机参数    随机生产的参数，便于开发者防止跨站攻击

            //POST https://bgm.tv/oauth/access_token
            //Parameter Type    Desc Note    Required
            //grant_type  string 授权方式    此处应使用 refresh_token	☑️
            //client_id   string App ID 注册应用时获取	☑️
            //client_secret   string App Secret 注册应用时获取	☑️
            //refresh_token   string Refresh Token 之前获取的 refresh token	☑️
            //redirect_uri    string 回调 URL 在后台设置的回调地址	☑️
            using (var httpClient = new HttpClient(new HttpClientHandler()
            {

            })
            {

            })
            {
                var httprequest = new HttpRequestMessage(HttpMethod.Get, "https://bgm.tv/oauth/authorize");
                var content = new MultipartFormDataContent
                {
                    { new StringContent("client_id"), "client_id" }
                };

                //httprequest.Content = new MultipartFormDataContent()
                //await httpClient.SendAsync();

                string code;
                using (var listener = new HttpListener())
                {
                    listener.Prefixes.Add(bangumiAPIAccessTokenOptions.ListenUrl);
                    listener.Start();
                    var context = await listener.GetContextAsync();
                    code = context.Request.QueryString.Get("code");
                    if (string.IsNullOrEmpty(code))
                        throw new Exception("Authorization code is missing.");
                }
            }
        }
    }
}
