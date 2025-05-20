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
namespace Bangumi.API.NET
{
    public class BangumiAPIOptions
    {
        /// <summary>
        /// Bangumi API 的基础 URL，默认值为 https://api.bgm.tv/。<br/>
        /// </summary>
        /// <remarks>
        /// 可以使用自己的 API 服务器，或者使用代理服务器。<br/>
        /// </remarks>
        public string BaseURL { get; set; } = "https://api.bgm.tv/";

        /// <summary>
        /// 非浏览器的 API 使用者请指定一个带有开发者个人 ID 和应用名称的 User Agent。<br/>
        /// </summary>
        /// <remarks>
        /// <br/>
        /// 如果你的应用需要进行分发（如安卓客户端，或者某个语言的 SDK Library），请附上版本号。<br/>
        /// <br/>
        /// 如果你的应用是一个开源项目，请在 User Agent 附上项目主页。<br/>
        /// <br/>
        /// 各种请求库的默认 UA 可能会被禁用。<br/>
        /// <br/>
        /// 建议的 User Agent 比如：<br/>
        /// <code>czy0729/Bangumi/6.4.0 (Android) (http://github.com/czy0729/Bangumi)</code>
        /// 移动端 APP，请添加版本号。
        /// <code>trim21/bangumi-episode-ics(https://github.com/Trim21/bangumi-episode-calendar)</code>
        /// cloudflare workers，不进行分发，不需要添加版本号。
        /// <code>sai/my-private-project</code>
        /// 某人的私有项目。
        /// 请不要使用类似于 
        /// <code>database</code>
        /// <code>Bangumi/1.0</code>
        /// <code>Bangumi/1.3.13.0</code>
        /// 的 User Agent。
        /// </remarks>
        public string? UserAgent { get; set; }
    }
}
