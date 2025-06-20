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
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.API.NET
{
    public static class BangumiExtensions
    {
        /// <summary>
        /// Checks if the <see cref="IBangumiClient"/> is null and throws an <see cref="ArgumentNullException"/> if it is.
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IBangumiClient ThrowIfNull(this IBangumiClient bangumiClient) =>
            bangumiClient ?? throw new ArgumentNullException(nameof(bangumiClient));

        #region openapi: 3.0.0

        /// <summary>
        /// 每日放送
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task<GetCalendarResponses> GetCalendar(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());

        /// <summary>
        /// 条目搜索
        /// </summary>
        /// <remarks>
        /// 推荐使用 <see cref="SearchSubjects"/>
        /// </remarks>
        /// <param name="bangumiClient"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Obsolete("这个接口为 3.0.0 版本接口，推荐使用 3.0.2 版本接口 SearchSubjects", false)]
        public static async Task<SearchSubjectByKeywordsResponses> SearchSubjectByKeywords(this IBangumiClient bangumiClient, string keyword,
            int? maxResults = null, int? start = null, SubjectType? subjectType = null, ResponseGroup responseGroup = ResponseGroup.Small) =>
            await bangumiClient.ThrowIfNull().SendRequest(new SearchSubjectByKeywordsRequest(keyword)
            {
                MaxResults = maxResults,
                ResponseGroup = responseGroup,
                Start = start,
                Type = subjectType,
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
        public static async Task GetRelatedPersonsByCharacterId(this IBangumiClient bangumiClient, int character_id) =>
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
        public static async Task UncollectCharacterByCharacterIdAndUserId(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new UncollectCharacterByCharacterIdAndUserIdRequest(id));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetPersonById(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetPersonByIdRequest(id));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetPersonImageById(this IBangumiClient bangumiClient, int id) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetPersonByIdRequest(id));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetRelatedSubjectsByPersonId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetRelatedCharactersByPersonId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task CollectPersonByPersonIdAndUserId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task UncollectPersonByPersonIdAndUserId(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserByName(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserAvatarByName(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetMyself(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bangumiClient"></param>
        /// <returns></returns>
        public static async Task GetUserCollectionsByUsername(this IBangumiClient bangumiClient) =>
            await bangumiClient.ThrowIfNull().SendRequest(new GetCalendarRequest());
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
