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
using Bangumi.API.NET.Requests.Abstractions;
using Bangumi.API.NET.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.API.NET.Requests.Subjects
{
    public class GetSubjectImageByIdRequest : RequestBase<string>
    {
        [JsonIgnore]
        private readonly Encoding _encoding = Encoding.UTF8;

        public GetSubjectImageByIdRequest(SubjectID subjectID, ImagesType imageType, Encoding? encoding = null) : base($"subjects/{subjectID}/image", HttpMethod.Get)
        {
            ImageType = imageType;
            if (encoding != null)
                _encoding = encoding;
        }

        [JsonIgnore]
        public ImagesType ImageType { get; set; }

        public override void MakeRequestQuery(Dictionary<string, string> query) =>
            query.Add("type", ImageType.ToString().ToLower());

        public override async Task<string> ParseResponse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync(), _encoding);
                return await streamReader.ReadToEndAsync();
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Moved || httpResponseMessage.StatusCode == HttpStatusCode.MovedPermanently
                || httpResponseMessage.StatusCode == HttpStatusCode.Found || httpResponseMessage.StatusCode == HttpStatusCode.Redirect)
            {
                var url = httpResponseMessage.Headers.Location.ToString();
                return url;
            }
            throw new Exception(MethodName + " failed: " + httpResponseMessage.StatusCode.ToString());
        }
    }
}
