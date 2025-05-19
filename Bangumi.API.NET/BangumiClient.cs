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
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using static Bangumi.API.NET.IBangumiClient;

namespace Bangumi.API.NET
{
    public class BangumiClient : IBangumiClient
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        private HttpClient HttpClient { get; }

        private HttpClientHandler HttpClientHandler { get; }

        private BangumiAPIOptions? Options { get; }

        private readonly CancellationTokenSource CancellationTokenSource;

        public event AsyncEventHandler<HttpRequestMessage>? SendingRequest;
        public event AsyncEventHandler<HttpResponseMessage>? ReceivedResponse;

        public BangumiClient(BangumiAPIOptions? options = null)
        {
            Options = options;

            HttpClientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
            };

            HttpClient = new HttpClient(HttpClientHandler)
            {
                Timeout = Timeout,
            };

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Empty);

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
