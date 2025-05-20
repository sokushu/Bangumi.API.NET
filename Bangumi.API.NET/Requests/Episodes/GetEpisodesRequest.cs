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
using Bangumi.API.NET.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Bangumi.API.NET.Requests.Episodes
{
    public class GetEpisodesRequest : PagedRequestBase<Paged_Episode>
    {
        public GetEpisodesRequest(SubjectID subjectID) : base("episodes", HttpMethod.Get)
        {
            SubjectID = subjectID;
        }

        [JsonIgnore]
        public EpType? Type { get; set; }

        [JsonIgnore]
        public SubjectID SubjectID { get; set; }

        public override void MakeRequestQuery(Dictionary<string, string> query)
        {
            base.MakeRequestQuery(query);
            if (Type.HasValue)
                query.Add("type", ((int)Type.Value).ToString());
            if (SubjectID != default)
                query.Add("subject_id", SubjectID.ToString());
        }
    }
}
