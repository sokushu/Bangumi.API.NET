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
using Bangumi.API.NET.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bangumi.API.NET.Types
{
    public class Infobox
    {
        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;

        [JsonConverter(typeof(InfoBoxItemConverter))]
        [JsonProperty("value")]
        public InfoboxItem? Value { get; set; }
    }

    public class InfoboxItem
    {
        public List<InfoboxItemKV> Items { get; set; } = new List<InfoboxItemKV>();

        public static implicit operator InfoboxItem(string value)
        {
            var item = new InfoboxItem();
            item.Items.Add(new InfoboxItemKV
            {
                Key = string.Empty,
                Value = value
            });
            return item;
        }

        public static implicit operator string(InfoboxItem item)
        {
            if (item.Items.Count == 0)
                return string.Empty;
            if (item.Items.Count == 1)
                return item.Items[0].Value ?? string.Empty;
            else
            {
                var sb = new StringBuilder();
                item.Items.ForEach(i => sb.AppendLine($@"""{i.Key}"": ""{i.Value}"""));
                return sb.ToString();
            }
        }

        public override string ToString() => (string)this;
    }

    public class InfoboxItemKV
    {
        [JsonProperty("k")]
        public string? Key { get; set; } = string.Empty;

        [JsonProperty("v")]
        public string? Value { get; set; } = string.Empty;
    }
}