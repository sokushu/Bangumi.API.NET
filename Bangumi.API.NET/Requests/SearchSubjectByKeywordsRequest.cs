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
using Bangumi.API.NET.Responses;
using Bangumi.API.NET.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Bangumi.API.NET.Requests
{
    public class SearchSubjectByKeywordsRequest : RequestBase<SearchSubjectByKeywordsResponses>
    {
        public SearchSubjectByKeywordsRequest(string keywords) : base($"search/subject/{keywords}", HttpMethod.Get) =>
            ApiVersion = string.Empty;

        /// <summary>
        /// 条目类型
        /// </summary>
        [JsonIgnore]
        public SubjectType? Type { get; set; }

        /// <summary>
        /// 返回数据大小
        /// </summary>
        [JsonIgnore]
        public ResponseGroup? ResponseGroup { get; set; }

        /// <summary>
        /// 开始条数
        /// </summary>
        [JsonIgnore]
        public int? Start { get; set; }
        
        [JsonIgnore]
        private int? _maxResults;

        /// <summary>
        /// 每页条数
        /// </summary>
        [JsonIgnore]
        public int? MaxResults
        {
            get => _maxResults;
            set
            {
                if (value > 25)
                    value = 25;
                if (value < 1)
                    value = 1;
                _maxResults = value;
            }
        }

        public override void MakeRequestQuery(Dictionary<string, string> query)
        {
            if (Type.HasValue)
                query.Add("type", ((int)Type.Value).ToString());
            if (ResponseGroup != null)
                query.Add("responseGroup", ResponseGroup.Value.ToString().ToLower());
            if (Start.HasValue)
                query.Add("start", Start.Value.ToString());
            if (MaxResults.HasValue)
                query.Add("max_results", MaxResults.Value.ToString());
        }
    }

    public enum ResponseGroup
    {
        Small,
        Medium,
        Large,
    }
}
