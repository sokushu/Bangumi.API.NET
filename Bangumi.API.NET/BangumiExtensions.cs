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
using Bangumi.API.NET.Requests;
using Bangumi.API.NET.Requests.Characters;
using Bangumi.API.NET.Requests.Episodes;
using Bangumi.API.NET.Requests.Persons;
using Bangumi.API.NET.Requests.Search;
using Bangumi.API.NET.Requests.Subjects;
using Bangumi.API.NET.Responses;
using Bangumi.API.NET.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.API.NET
{
    /// <summary>
    /// Bangumi.API.NET - 用 C# 召唤 Bangumi API 的魔法库
    /// </summary>
    /// <remarks>
    /// 爱用C#的有福了！<br/>
    /// 在遥远的二次元星系，Bangumi API 化身为神秘的能量源，唯有掌握 C# 之力的勇者才能召唤。<br/>
    /// 现在，我用代码织成魔法阵，召唤出 Bangumi C# API 库——<br/>
    /// 让你用一行 await，穿梭番剧宇宙，<br/>
    /// 用一行 lambda，收集你的本命角色，<br/>
    /// 用一行 LINQ，筛选你的追番清单。<br/>
    /// 别再用 Postman 手搓请求了，<br/>
    /// 让 Bangumi.API.NET 带你体验“用 C# 拯救世界”的快乐！<br/>
    /// 快来试试，和我一起用代码点亮二次元吧！<br/>
    /// </remarks>
    public static class BangumiExtensions
    {
        /// <summary>
        /// Ensures that the specified <see cref="IBangumiClient"/> instance is not null.
        /// </summary>
        /// <param name="bangumiClient">The <see cref="IBangumiClient"/> instance to validate.</param>
        /// <returns>The provided <see cref="IBangumiClient"/> instance if it is not null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="bangumiClient"/> is <see langword="null"/>.</exception>
        private static IBangumiClient ThrowIfNull(this IBangumiClient bangumiClient) =>
            bangumiClient ?? throw new ArgumentNullException(nameof(bangumiClient));

        /// <summary>
        /// Converts the specified object to its string representation.
        /// </summary>
        /// <param name="obj">The object to convert. If <paramref name="obj"/> is <see langword="null"/>, an empty string is returned.</param>
        /// <returns>The string representation of the object, or an empty string if <paramref name="obj"/> is <see
        /// langword="null"/>.</returns>
        private static string CastToString(this object? obj)
        {
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }

        #region openapi: 3.0.0

        /// <summary>
        /// Retrieves the calendar of upcoming episodes and events from the Bangumi service.
        /// </summary>
        /// <remarks>This method sends a GET request to the Bangumi API to retrieve the calendar data.
        /// Ensure that the <paramref name="bangumiClient"/> is properly initialized before calling this
        /// method.</remarks>
        /// <param name="bangumiClient">The <see cref="IBangumiClient"/> instance used to send the request.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a  <see
        /// cref="GetCalendarResponses"/> object with the calendar data.</returns>
        public static async Task<GetCalendarResponses> GetCalendar(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new FunctionRequest<GetCalendarResponses>("/calendar", HttpMethod.Get)
            {

            });

        /// <summary>
        /// Searches for subjects based on the specified keyword and optional filters.
        /// </summary>
        /// <remarks>This method is marked as obsolete. It is recommended to use the <see
        /// cref="SearchSubjects"/> method instead. The search results can be filtered by subject type,
        /// maximum number of results, starting index, and response group.</remarks>
        /// <param name="bangumiClient">The Bangumi client instance used to perform the search. Cannot be null.</param>
        /// <param name="keyword">The keyword to search for. Cannot be null or empty.</param>
        /// <param name="maxResults">The maximum number of results to return. If null, the default value is used.</param>
        /// <param name="start">The starting index for the search results. If null, the default value is used.</param>
        /// <param name="subjectType">The type of subjects to filter the search results by. If null, all types are included.</param>
        /// <param name="responseGroup">The level of detail to include in the response. Defaults to <see cref="ResponseGroup.Small"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the search results as a <see
        /// cref="SearchSubjectByKeywordsResponses"/> object.</returns>
        [Obsolete("推荐使用接口 SearchSubjects", false)]
        public static async Task<SearchSubjectByKeywordsResponses> SearchSubjectByKeywords(this IBangumiClient bangumiClient, string keyword,
            int? maxResults = null, int? start = null, SubjectType? subjectType = null, ResponseGroup responseGroup = ResponseGroup.Small) =>

            await bangumiClient.ThrowIfNull().SendRequest(new FunctionRequest<SearchSubjectByKeywordsResponses>($"/search/subject/{keyword}", HttpMethod.Get)
            {
                MakeRequestQueryAction = (query) =>
                {
                    query.Add("type", subjectType.CastToString());
                    query.Add("start", start.CastToString());
                    query.Add("max_results", maxResults.CastToString());
                    query.Add("responseGroup", responseGroup.CastToString());
                },
            });

        #endregion

        #region openapi: 3.0.2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<Paged_Subject> SearchSubjects(this IBangumiClient bangumiClient, string keyword, int? limit = null, int? offset = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new SearchSubjectsRequest(keyword)
            {
                Limit = limit,
                Offset = offset
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<Paged_Character> SearchCharacters(this IBangumiClient bangumiClient, string keyword, int? limit = null, int? offset = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new SearchCharactersRequest(keyword)
            {
                Limit = limit,
                Offset = offset
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <param name="keyword"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<Paged_Person> SearchPersons(this IBangumiClient bangumiClient, string keyword, int? limit = null, int? offset = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new SearchPersonsRequest(keyword)
            {
                Limit = limit,
                Offset = offset
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Paged_Subject> GetSubjects(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetSubjectsRequest(SubjectType.Book));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Subject> GetSubjectById(this IBangumiClient bangumiClient, SubjectID subjectID) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetSubjectByIdRequest(subjectID));

        /// <summary>
        /// 获取条目图片
        /// </summary>
        /// <remarks>
        /// 获取URL地址，或者获取通过Encoding编码后的图片数据
        /// </remarks>
        /// <param name="bangumiClient"></param>
        /// <param name="subjectID"></param>
        /// <param name="imageType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> GetSubjectImageById(this IBangumiClient bangumiClient, SubjectID subjectID, ImagesType imageType, Encoding? encoding = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetSubjectImageByIdRequest(subjectID, imageType, encoding));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static async Task<RelatedPerson> GetRelatedPersonsBySubjectId(this IBangumiClient bangumiClient, SubjectID subjectID) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedPersonsBySubjectIdRequest(subjectID));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static async Task<RelatedCharacter> GetRelatedCharactersBySubjectId(this IBangumiClient bangumiClient, SubjectID subjectID) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedCharactersBySubjectIdRequest(subjectID));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetRelatedSubjectsBySubjectId(this IBangumiClient bangumiClient, SubjectID subjectID) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedSubjectsBySubjectIdRequest(subjectID));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Paged_Episode> GetEpisodes(this IBangumiClient bangumiClient, SubjectID subjectID, EpType? epType = null, int? limit = null, int? offset = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetEpisodesRequest(subjectID)
            {
                Type = epType,
                Limit = limit,
                Offset = offset,
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Episode> GetEpisodeById(this IBangumiClient bangumiClient, int episode_id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetEpisodeByIdRequest(episode_id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Character> GetCharacterById(this IBangumiClient bangumiClient, int character_id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCharacterByIdRequest(character_id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetCharacterImageById(this IBangumiClient bangumiClient, int character_id, ImagesType? imagesType = null, Encoding? encoding = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCharacterImageByIdRequest(character_id)
            {
                Type = imagesType,
                Encoding = encoding ?? Encoding.UTF8,
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<RelatedSubject> GetRelatedSubjectsByCharacterId(this IBangumiClient bangumiClient, int character_id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedSubjectsByCharacterIdRequest(character_id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<CharacterPerson> GetRelatedPersonsByCharacterId(this IBangumiClient bangumiClient, int character_id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedPersonsByCharacterIdRequest(character_id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<bool> CollectCharacterByCharacterIdAndUserId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new CollectCharacterByCharacterIdAndUserIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<bool> UncollectCharacterByCharacterIdAndUserId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new UncollectCharacterByCharacterIdAndUserIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<Person> GetPersonById(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetPersonByIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<string> GetPersonImageById(this IBangumiClient bangumiClient, int id, ImagesType imagesType) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetPersonImageByIdRequest(id, imagesType));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<List<RelatedSubject>> GetRelatedSubjectsByPersonId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedSubjectsByPersonIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<CharacterPerson> GetRelatedCharactersByPersonId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetRelatedCharactersByPersonIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<bool> CollectPersonByPersonIdAndUserId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new CollectPersonByPersonIdAndUserIdRequest(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task UncollectPersonByPersonIdAndUserId(this IBangumiClient bangumiClient, int person_id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new CheckFunctionRequest($"persons/{person_id}/collect", HttpMethod.Delete)
            {
                HttpStatusCodes = new HashSet<HttpStatusCode>
                 {
                     HttpStatusCode.NoContent,
                 },
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserByName(this IBangumiClient bangumiClient, string username) =>
            await bangumiClient.ThrowIfNull().SendRequest(new FunctionRequest<User>($"users/{username}", HttpMethod.Get));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserAvatarByName(this IBangumiClient bangumiClient, string username, ImagesType imagesType) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetPhotoFunctionRequest($"users/{username}/avatar", HttpMethod.Get)
            {
                MakeRequestQueryAction = (query) =>
                {
                    query.Add("type", imagesType.ToString().ToLower());
                }
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<User> GetMyself(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new FunctionRequest<User>("me", HttpMethod.Get));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserCollectionsByUsername(this IBangumiClient bangumiClient, string username, SubjectType? subjectType = null, SubjectCollectionType? collectionType = null) =>
            await bangumiClient.ThrowIfNull().SendRequest(new FunctionRequest<Paged_UserCollection>($"users/{username}/collections", HttpMethod.Get)
            {
                MakeRequestQueryAction = (query) =>
                {
                    if (subjectType != null)
                        query.Add("subject_type", subjectType.ToString().ToLower());
                    if (collectionType != null)
                        query.Add("type", collectionType.ToString().ToLower());
                }
            });
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task PostUserCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task PatchUserCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserSubjectEpisodeCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task PatchUserSubjectEpisodeCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserEpisodeCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task PutUserEpisodeCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserCharacterCollections(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserCharacterCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserPersonCollections(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserPersonCollection(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetPersonRevisions(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetPersonRevisionByRevisionId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetCharacterRevisions(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetCharacterRevisionByRevisionId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetSubjectRevisions(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetSubjectRevisionByRevisionId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetEpisodeRevisions(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetEpisodeRevisionByRevisionId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task NewIndex(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetIndexById(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task EditIndexById(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetIndexSubjectsByIndexId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task AddSubjectToIndexByIndexId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task EditIndexSubjectsByIndexIdAndSubjectID(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task DelelteSubjectFromIndexByIndexIdAndSubjectID(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task CollectIndexByIndexIdAndUserId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task UncollectIndexByIndexIdAndUserId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        #endregion
    }
}
