using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WonderApp.Data;
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
        /// There is an algorthim in place which takes 10 random Wonders from the following: 
        /// proximity of 5 miles, priority and popularity. It also includes 10 completely random
        /// wonders. There will be no duplicates in the list and user will not see wonders 
        /// they have previously disliked OR wonders they have previously liked.
        /// If no location just returns priority, random and popular.
        /// Returns HTTP StatusCode 200 with JSON list of wonder deals.
        /// If error, return Http Status Code 500 with error message.
        /// </summary>
        /// <returns></returns>        
        public async Task<HttpResponseMessage> PostWonders([FromBody]WonderModel model)
        {
            try
            {
                if (model.UserId != null && DataContext.AspNetUsers.All(x => x.Id != model.UserId))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "This user is not recognised");
                }

                var wonders = new List<DealModel>();

                if (model.Latitude != null && model.Longitude != null)
                {
                    wonders = await Task.Run(() =>
                    {
                        var nearestWonders = GetNearestWonders(model);
                        var priorityWonders = GetPriorityWonders(model);

                        var popularWonders = GetPopularWonders(model, amountToSkip: 0);
                        var amountToSkip = 100;
                        while (popularWonders.Count() == 0)
                        {
                            if (NoMoreWonders(model, amountToSkip)) break;
                            popularWonders = GetPopularWonders(model, amountToSkip);
                            amountToSkip += 100;
                        }

                        var randomWonders = GetRandomWonders(model);

                        var results = nearestWonders.Union(priorityWonders).Union(popularWonders).Union(randomWonders);
                        results = results.OrderBy(x => Guid.NewGuid());

                        return Mapper.Map<List<DealModel>>(results);
                    });
                }
                else
                {
                    wonders = await Task.Run(() =>
                    {
                        var priorityWonders = GetPriorityWonders(model);

                        var popularWonders = GetPopularWonders(model, amountToSkip: 0);
                        var amountToSkip = 100;
                        while (popularWonders.Count() == 0)
                        {
                            if (NoMoreWonders(model, amountToSkip)) break;
                            popularWonders = GetPopularWonders(model, amountToSkip);
                            amountToSkip += 100;
                        }

                        var randomWonders = GetRandomWonders(model);

                        var results = priorityWonders.Union(popularWonders).Union(randomWonders);
                        results = results.OrderBy(x => Guid.NewGuid());

                        return Mapper.Map<List<DealModel>>(results);
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private bool NoMoreWonders(WonderModel model, int amountToSkip)
        {
            var wonders = DataContext.Deals
            .Where(w => w.CityId == model.CityId
                && w.Archived == false
                && w.Expired != true
                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                && w.MyWonderUsers.All(u => u.Id != model.UserId))
            .OrderByDescending(w => w.Likes)
            .Skip(amountToSkip)
            .Take(100);

            return wonders.Count() == 0;
        }

        private IQueryable<Data.Deal> GetNearestWonders(WonderModel model)
        {
            var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);
            return DataContext.Deals
                           .Where(w => w.Location.Geography.Distance(usersPosition) * .00062 <= WonderAppConstants.DefaultRadius
                               && w.Archived == false
                               && w.Expired != true
                               && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                               && w.MyRejectUsers.All(u => u.Id != model.UserId)
                               && w.MyWonderUsers.All(u => u.Id != model.UserId))
                           .OrderBy(x => Guid.NewGuid())
                           .Take(10);
        }

        private IQueryable<Data.Deal> GetPriorityWonders(WonderModel model)
        {
            return DataContext.Deals
                            .Where(w => w.Priority == true
                                && w.CityId == model.CityId
                                && w.Archived == false
                                && w.Expired != true
                                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(10);
        }

        private IQueryable<Data.Deal> GetPopularWonders(WonderModel model, int amountToSkip)
        {
            return DataContext.Deals
            .Where(w => w.CityId == model.CityId
                && w.Archived == false
                && w.Expired != true
                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                && w.MyWonderUsers.All(u => u.Id != model.UserId))
            .OrderByDescending(w => w.Likes)
            .Skip(amountToSkip)
            .Take(100)
            .OrderBy(x => Guid.NewGuid())
            .Take(10);
        }

        private IQueryable<Data.Deal> GetRandomWonders(WonderModel model)
        {
            return DataContext.Deals
                            .Where(w => w.CityId == model.CityId
                                && w.Archived == false
                                && w.Expired != true
                                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(10);
        }

        /// <summary>
        /// Return all Wonders that the user has liked
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("myWonders/{userId}")]
        public async Task<HttpResponseMessage> GetMyWonders(string userId)
        {
            try
            {
                var wonders = await Task.Run(() =>
                {
                    return Mapper.Map<List<DealModel>>(DataContext.AspNetUsers.Where(u => u.Id == userId).SelectMany(w => w.MyWonders));
                });
                
                return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Return wonderfied Cities with Latitude and Longitude
        /// </summary>
        /// <returns></returns>
        [Route("cities")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCities()
        {
            var listOfCities = await Task.Run(() => { return Mapper.Map<List<CityModel>>(DataContext.Cities); });
            return Request.CreateResponse(HttpStatusCode.OK, listOfCities);
        }



        /// <summary>
        /// Add a Wonder to a Users "MyWonders" collection
        /// Returns new MyWonders collection
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wonderId"></param>
        /// <returns></returns>
        [Route("like/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> Like(string userId, int wonderId)
        {
            try
            {
                var user = DataContext.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                var deal = DataContext.Deals.FirstOrDefault(w => w.Id == wonderId);
                if (deal != null && user != null)
                {
                    //Should never be true - but belt and braces :D
                    if (user.MyRejects.Contains(deal))
                    {
                        user.MyRejects.Remove(deal);
                    }

                    if (!user.MyWonders.Contains(deal))
                    {
                        user.MyWonders.Add(deal);
                        deal.Likes++;
                    }

                    
                    DataContext.Commit();

                    //Return list of MyWonders 
                    var wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(user.MyWonders);
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, wonders);
                }

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please supply a valid userId and wonderId");
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Add a Wonder to a Users "MyRejects" collection
        /// Returns new MyRejects collection
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wonderId"></param>
        /// <returns></returns>
        [Route("dislike/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> Dislike(string userId, int wonderId)
        {
            try
            {
                var user = DataContext.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                var deal = DataContext.Deals.FirstOrDefault(w => w.Id == wonderId);
                if (deal != null && user != null)
                {
                    //Should never be true - but belt and braces :D
                    if (user.MyWonders.Contains(deal))
                    {
                        user.MyWonders.Remove(deal);  
                    }

                    if (!user.MyRejects.Contains(deal))
                    {
                        user.MyRejects.Add(deal);
                    }

                    
                    DataContext.Commit();

                    //Return list of MyWonders 
                    var wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(user.MyRejects);
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, wonders);
                }

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please supply a valid userId and wonderId");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Remove a Wonder from "MyWonders" collection
        /// Returns new MyWonders collection
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wonderId"></param>
        /// <returns></returns>
        [Route("removeLike/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> RemoveLike(string userId, int wonderId)
        {
            try
            {
                var user = DataContext.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                var deal = DataContext.Deals.FirstOrDefault(w => w.Id == wonderId);
                if (deal != null && user != null)
                {
                    if (user.MyWonders.Contains(deal))
                    {
                        user.MyWonders.Remove(deal);
                    }

                    DataContext.Commit();

                    //Return list of MyWonders 
                    var wonders = await Task.Run(() =>
                    {
                        return Mapper.Map<List<DealModel>>(user.MyRejects);
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, wonders);
                }

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please supply a valid userId and wonderId");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
