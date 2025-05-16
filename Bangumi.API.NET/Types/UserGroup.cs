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
    /// 用户组
    /// </summary>
    public enum UserGroup
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Bangumi 管理猿
        /// </summary>
        BangumiAdmin = 2,

        /// <summary>
        /// 天窗管理猿
        /// </summary>
        DoujinAdmin = 3,

        /// <summary>
        /// 禁言用户
        /// </summary>
        MutedUser = 4,

        /// <summary>
        /// 禁止访问用户
        /// </summary>
        BlockedUser = 5,

        /// <summary>
        /// 人物管理猿
        /// </summary>
        PersonAdmin = 8,

        /// <summary>
        /// 维基条目管理猿
        /// </summary>
        WikiAdmin = 9,

        /// <summary>
        /// 用户
        /// </summary>
        User = 10,

        /// <summary>
        /// 维基人
        /// </summary>
        WikiUser = 11,
    }
}
