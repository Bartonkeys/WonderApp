using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Constants;
using WonderApp.Models;
using WonderApp.Models.Helpers;

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

        /// <summary>
        /// Search for wonders by WonderModel (with tag ID string)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("wonders")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostWonderModel(WonderModel model)
        {
            try
            {
                var wonders = new List<DealModel>();
                
                if (model.Latitude != null && model.Longitude != null)
                {
                    var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);

                    wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(DataContext.Deals
                           .Where(w => w.Location.Geography.Distance(usersPosition) * .00062 <= WonderAppConstants.DefaultRadius
                               && w.Tags.Any(x => model.TagId == x.Id)
                               && !w.Archived.Value
                               && w.MyRejectUsers.All(u => u.Id != model.UserId)
                               && w.MyWonderUsers.All(u => u.Id != model.UserId))
                           .Take(WonderAppConstants.DefaultMaxNumberOfWonders));
                    });
                }
                else
                {
                    wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(DataContext.Deals
                            .Where(w => !w.Archived.Value 
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId)
                                && w.Tags.Any(x => model.TagId == x.Id))
                            .OrderByDescending(x => x.Id)
                            .Take(WonderAppConstants.DefaultMaxNumberOfWonders));
                    });

                }

                return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }



    }
}
