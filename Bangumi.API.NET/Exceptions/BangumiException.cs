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
using Bangumi.API.NET.Types;
using Newtonsoft.Json;
using System;

namespace Bangumi.API.NET.Exceptions
{
    public abstract class BangumiException : Exception
    {
        public ErrorDetail? ErrorDetail { get; }
        public BangumiException(ErrorDetail? errorDetail) =>
            ErrorDetail = errorDetail;

        public BangumiException(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            ErrorDetail = JsonConvert.DeserializeObject<ErrorDetail>(json);
        }

        public override string Message => ErrorDetail != null ? $"{ErrorDetail.Title}: {ErrorDetail.Description}" : base.Message;
    }
}
