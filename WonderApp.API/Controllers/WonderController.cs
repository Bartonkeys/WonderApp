using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WonderApp.Models;
using Elmah.Contrib.WebApi;
using WonderApp.Models.Helpers;
using WonderApp.Constants;

namespace WonderApp.Controllers
{
    /// <summary>
    /// The main API for communication with device
    /// </summary>
    [RoutePrefix("api/wonder")]
    public class WonderController : BaseApiController
    {
        /// <summary>
        /// HTTP POST to return wonder deals. Send the following in body: 
        /// Current position in latitude and longitude, cityId and userId.
        /// There is an algorthim in place which takes 6 random Wonders from the following: 
        /// proximity of 2 miles, priority and popularity. It also includes 2 completely random
        /// wonders. There will be no duplicates in the list and user will not see wonders 
        /// they have previously disliked.
        /// Returns HTTP StatusCode 200 with JSON list of wonder deals.
        /// If error, return Http Status Code 500 with error message.
        /// </summary>
        /// <returns></returns>        
        public async Task<HttpResponseMessage> PostWonders([FromBody]WonderModel model)
        {
            try
            {
                var wonders = new List<DealModel>();

                if (model.Latitude != null && model.Longitude != null)
                {
                    var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);

                    wonders = await Task.Run(() =>
                    {
                        //TODO Expiry Date and always available

                        var nearestWonders = DataContext.Deals
                           .Where(w => w.Location.Geography.Distance(usersPosition) * .00062 <= WonderAppConstants.DefaultRadius 
                               && !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
                           .OrderBy(x => Guid.NewGuid())
                           .Take(6);

                        var priorityWonders = DataContext.Deals
                            .Where(w => w.Priority.HasValue && w.Priority.Value && w.CityId == model.CityId 
                                && !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(6);

                        var popularWonders = DataContext.Deals
                            .Where(w => w.CityId == model.CityId && !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
                            .OrderByDescending(w => w.Likes)
                            .Take(50)
                            .OrderBy(x => Guid.NewGuid())
                            .Take(6);

                        var randomWonders = DataContext.Deals
                            .Where(w => w.CityId == model.CityId && !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(2);

                        var results = nearestWonders.Union(priorityWonders).Union(popularWonders).Union(randomWonders);
                        results = results.OrderBy(x => Guid.NewGuid());

                        return Mapper.Map<List<DealModel>>(results);
                    });
                }
                else
                {
                    wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(DataContext.Deals
                            .Where(w => !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
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

        [Route("cities")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCities()
        {
            var listOfCities = await Task.Run(() => { return Mapper.Map<List<CityModel>>(DataContext.Cities); });
            return Request.CreateResponse(HttpStatusCode.OK, listOfCities);
        }

    }
}
