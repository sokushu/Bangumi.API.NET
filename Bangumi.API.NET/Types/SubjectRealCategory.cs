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
namespace Bangumi.API.NET.Types
{
    /// <summary>
    /// 电影类型
    /// </summary>
    public enum SubjectRealCategory
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other = 0,
        /// <summary>
        /// 日剧
        /// </summary>
        JP = 1,
        /// <summary>
        /// 欧美剧
        /// </summary>
        EN = 2,
        /// <summary>
        /// 华语剧
        /// </summary>
        CN = 3,
        /// <summary>
        /// 电视剧
        /// </summary>
        TV = 6001,
        /// <summary>
        /// 电影
        /// </summary>
        Movie = 6002,
        /// <summary>
        /// 演出
        /// </summary>
        Live = 6003,
        /// <summary>
        /// 综艺
        /// </summary>
        Show = 6004,
    }
}
