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
using System.Collections.Generic;

namespace Bangumi.API.NET.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public SubjectType? Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name_cn")]
        public string? NameCN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("summary")]
        public string? Summary { get; set; }

        [JsonProperty("series")]
        public bool? Series { get; set; }

        [JsonProperty("nsfw")]
        public bool? Nsfw { get; set; }

        [JsonProperty("locked")]
        public bool? Locked { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }

        [JsonProperty("platform")]
        public string? Platform { get; set; }

        [JsonProperty("images")]
        public Images? Images { get; set; }

        [JsonProperty("infobox")]
        public List<Infobox>? Infobox { get; set; }

        [JsonProperty("volumes")]
        public int? Volumes { get; set; }

        [JsonProperty("eps")]
        public int? Eps { get; set; }

        [JsonProperty("total_episodes")]
        public int? TotalEpisodes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rating")]
        public SubjectRating? Rating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("collection")]
        public Collection? Collection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("tags")]
        public List<SubjectTags> Tags { get; set; } = new List<SubjectTags>();

        [JsonProperty("meta_tags")]
        public List<string> MetaTags { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("url")]
        public string? URL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("air_date")]
        public string? AirDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("air_weekday")]
        public int AirWeekday { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
