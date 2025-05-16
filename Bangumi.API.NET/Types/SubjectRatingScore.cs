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
using Newtonsoft.Json;

namespace Bangumi.API.NET.Types
{
    /// <summary>
    /// 各个分数段对应的评分人数
    /// </summary>
    [JsonObject("score")]
    public class SubjectRatingScore
    {
        /// <summary>
        /// 打1分的人数
        /// </summary>
        [JsonProperty("1")]
        public int _01 { get; set; }
        /// <summary>
        /// 打2分的人数
        /// </summary>
        [JsonProperty("2")]
        public int _02 { get; set; }
        /// <summary>
        /// 打3分的人数
        /// </summary>
        [JsonProperty("3")]
        public int _03 { get; set; }
        /// <summary>
        /// 打4分的人数
        /// </summary>
        [JsonProperty("4")]
        public int _04 { get; set; }
        /// <summary>
        /// 打5分的人数
        /// </summary>
        [JsonProperty("5")]
        public int _05 { get; set; }
        /// <summary>
        /// 打6分的人数
        /// </summary>
        [JsonProperty("6")]
        public int _06 { get; set; }
        /// <summary>
        /// 打7分的人数
        /// </summary>
        [JsonProperty("7")]
        public int _07 { get; set; }
        /// <summary>
        /// 打8分的人数
        /// </summary>
        [JsonProperty("8")]
        public int _08 { get; set; }
        /// <summary>
        /// 打9分的人数
        /// </summary>
        [JsonProperty("9")]
        public int _09 { get; set; }
        /// <summary>
        /// 打10分的人数
        /// </summary>
        [JsonProperty("10")]
        public int _10 { get; set; }
    }
}
