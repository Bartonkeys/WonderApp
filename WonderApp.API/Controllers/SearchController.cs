using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Models;

namespace WonderApp.Controllers
{
    /// <summary>
    /// Handle all search queries
    /// </summary>
    [RoutePrefix("api/search")]
    public class SearchController : BaseApiController
    {
        /// <summary>
        /// Search for tags
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Route("tags/{searchString}")]
        public async Task<HttpResponseMessage> GetTags(string searchString)
        {
            var tags = await Task.Run(() => { return 
                Mapper.Map<List<TagModel>>(DataContext.Tags.Where(t => t.Name.StartsWith(searchString))); });

            return Request.CreateResponse(HttpStatusCode.OK, tags);
        }
    }
}
