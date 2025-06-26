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
using System.Collections.Generic;

namespace Bangumi.API.NET.Requests
{
    /// <summary>
    /// Represents a collection of key-value pairs where keys are strings and values are strings.
    /// </summary>
    /// <remarks>This class extends <see cref="Dictionary{TKey, TValue}"/> to provide additional
    /// functionality,  such as preventing the addition of null or empty values. It is useful for scenarios where  query
    /// parameters or other string-based key-value pairs need to be managed with stricter  validation rules.</remarks>
    public class QueryDictionary : Dictionary<string, string>
    {
        /// <summary>
        /// Adds a key-value pair to the collection if the value is not null or empty.
        /// </summary>
        /// <remarks>This method overrides the base implementation to ensure that only non-null and
        /// non-empty values are added to the collection.</remarks>
        /// <param name="key">The key associated with the value to add. Cannot be null.</param>
        /// <param name="value">The value to associate with the key. If null or empty, the pair will not be added.</param>
        public new void Add(string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
                base.Add(key, value);
        }
    }
}
