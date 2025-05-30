﻿//MIT License

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
    /// 
    /// </summary>
    public class Character
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public CharacterType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("images")]
        public PersonImages? Images { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("gender")]
        public string? Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("blood_type")]
        public BloodType? BloodType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("birth_year")]
        public int? BirthYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("birth_mon")]
        public int? BirthMonth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("birth_day")]
        public int? BirthDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("stat")]
        public Stat Stat { get; set; } = new Stat();
    }
}
