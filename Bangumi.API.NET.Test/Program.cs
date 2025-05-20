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
using Bangumi.API.NET;
using Bangumi.API.NET.Types;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IBangumiClient bangumiClient = new BangumiClient(new BangumiAPIOptions
        {
            UserAgent = "trim21/bangumi-episode-ics (https://github.com/Trim21/bangumi-episode-calendar)"
        });
        bangumiClient.ReceivedResponse += BangumiClient_ReceivedResponse;

        var result001 = await bangumiClient.GetEpisodes(123456);
        var result002 = await bangumiClient.GetSubjectImageById(123456, ImagesType.Common);
    }

    private static async Task BangumiClient_ReceivedResponse(object? sender, HttpResponseMessage e)
    {
        var json = await e.Content.ReadAsStringAsync();
    }
}