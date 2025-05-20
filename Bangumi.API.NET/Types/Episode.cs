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
    public class Episode
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("type")]
        public EpType Type { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("name_cn")]
        public string? NameCN { get; set; }

        [JsonProperty("sort")]
        public float? Sort { get; set; }

        [JsonProperty("ep")]
        public float? Ep { get; set; }

        [JsonProperty("airdate")]
        public string? Airdate { get; set; }

        [JsonProperty("comment")]
        public int Comment { get; set; }

        [JsonProperty("duration")]
        public string? Duration { get; set; }

        [JsonProperty("desc")]
        public string? Desc { get; set; }

        [JsonProperty("disc")]
        public int Disc { get; set; }

        [JsonProperty("duration_seconds")]
        public int DurationSeconds { get; set; }

        [JsonProperty("subject_id")]
        public SubjectID SubjectID { get; set; }
    }
}
