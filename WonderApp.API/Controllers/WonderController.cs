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
    public class WonderController : BaseApiController
    {
        /// <summary>
        /// HTTP POST to return wonder deals. Send the following in body: 
        /// Current position in latitude and longitude, city, along with radius of search and maximum number of wonders to return.
        /// If no location will return most recent wonders up to max no. of wonder deals.
        /// If no radius then will default to 5 miles (unless location isn't there).
        /// if no max number of wonders, will default to 20.
        /// Returns HTTP StatusCode 200 with JSON list of wonder deals.
        /// If error, return Http Status Code 500 with error message.
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostWonders([FromBody]WonderModel model)
        {
            try
            {
                var wonders = new List<DealModel>();

                model.RadiusInMiles = model.RadiusInMiles ?? WonderAppConstants.DefaultRadius;
                model.MaxWonders = model.MaxWonders ?? WonderAppConstants.DefaultMaxNumberOfWonders;

                if (model.Latitude != null && model.Longitude != null)
                {
                    var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);

                    wonders = await Task.Run(() =>
                    {
                        var nearestWonders = DataContext.Deals
                           .Where(w => w.Location.Geography.Distance(usersPosition)*.00062 <= model.RadiusInMiles && !w.Archived && w.MyRejectUsers.All(u => u.Id != model.UserId))
                           .OrderBy(x => Guid.NewGuid())
                           .Take(6);

                        var priorityWonders = DataContext.Deals
                            .Where(w => w.Priority.HasValue && w.Priority.Value && w.CityId == model.CityId && w.MyRejectUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(6);

                        var popularWonders = DataContext.Deals
                            .Where(w => w.CityId == model.CityId && w.MyRejectUsers.All(u => u.Id != model.UserId))
                            .OrderByDescending(w => w.Likes)
                            .Take(50)
                            .OrderBy(x => Guid.NewGuid())
                            .Take(6);

                        var randomWonders = DataContext.Deals
                            .Where(w => w.CityId == model.CityId && w.MyRejectUsers.All(u => u.Id != model.UserId))
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
                            .Take(model.MaxWonders.Value));
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
