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
        private List<int> _categories;

        /// <summary>
        /// HTTP POST to return wonder deals. Send the following in body: 
        /// Current position in latitude and longitude, cityId and userId.
        /// If there are unseen priority wonders for city then it returns all of them.
        /// If there are no priority wonders then it returns useen wonders from the following:
        /// Proximity of 1 mile, 3 miles, popularity and random. 
        /// There will be no duplicates in the list.
        /// If no location just returns random and popular.
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

                SetUserCategories(model.UserId);

                var wonders = new List<DealModel>();

                wonders = await Task.Run(() =>
                {
                    var priorityWonders = GetPriorityWonders(model).ToList();
                    return Mapper.Map<List<DealModel>>(priorityWonders);
                });

                if (wonders.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, wonders);

                if (model.Latitude != null && model.Longitude != null)
                {
                    wonders = await Task.Run(() =>
                    {
                        var popularWonders = GetPopularWonders(model, numberToTake: WonderAppConstants.DefaultNumberOfWondersToTake);
                        var randomWonders = GetRandomWonders(model, numberToTake: WonderAppConstants.DefaultNumberOfWondersToTake);
                        var oneMileWonders = GetNearestWonders(model, mileRadiusFrom: 0, mileRadiusTo: 1,
                            amountToTake: WonderAppConstants.DefaultNumberOfWondersToTake);
                        var threeMileWonders = GetNearestWonders(model, mileRadiusFrom: 1, mileRadiusTo: 3,
                            amountToTake: WonderAppConstants.DefaultNumberOfWondersToTake);

                        var results = oneMileWonders.Union(threeMileWonders).Union(popularWonders).Union(randomWonders);
                        results = results.OrderBy(x => Guid.NewGuid());

                        return Mapper.Map<List<DealModel>>(results);

                    });
                }
                else
                {
                    wonders = await Task.Run(() =>
                    {
                        var popularWonders = GetPopularWonders(model, numberToTake: WonderAppConstants.DefaultNumberOfWondersToTake * 2);
                        var randomWonders = GetRandomWonders(model, numberToTake: WonderAppConstants.DefaultNumberOfWondersToTake * 2);

                        var results = popularWonders.Union(randomWonders);
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

        /// <summary>
        /// Return list of all priority wonders that user has not seen for specified city.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("priority")]
        public async Task<HttpResponseMessage> PostPriorityWonders([FromBody]WonderModel model)
        {
            try
            {
                if (model.UserId != null && DataContext.AspNetUsers.All(x => x.Id != model.UserId))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "This user is not recognised");
                }

                SetUserCategories(model.UserId);

                var wonders = new List<DealModel>();
                wonders = await Task.Run(() =>
                {
                    var results = GetPriorityWonders(model);
                    return Mapper.Map<List<DealModel>>(results);
                });


                return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("popular/{take}")]
        public async Task<HttpResponseMessage> PostPopularWonders(int take, [FromBody]WonderModel model)
        {
            try
            {
                if (model.UserId != null && DataContext.AspNetUsers.All(x => x.Id != model.UserId))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "This user is not recognised");
                }

                SetUserCategories(model.UserId);

                var wonders = new List<DealModel>();
                wonders = await Task.Run(() =>
                {
                    var results = GetPopularWonders(model, take);
                    return Mapper.Map<List<DealModel>>(results);
                });


                return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("nearest/{radiusFrom}/{radiusTo}")]
        public async Task<HttpResponseMessage> PostNearestWonders(int radiusFrom, int radiusTo, [FromBody]WonderModel model)
        {
            try
            {
                if (model.UserId != null && DataContext.AspNetUsers.All(x => x.Id != model.UserId))
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "This user is not recognised");

                SetUserCategories(model.UserId);

                if (model.Latitude != null && model.Longitude != null)
                {
                    var wonders = new List<DealModel>();
                    wonders = await Task.Run(() =>
                    {
                        var results = GetNearestWonders(model, mileRadiusFrom: radiusFrom, mileRadiusTo: radiusTo, amountToTake: WonderAppConstants.DefaultNumberOfWondersToTake);
                        return Mapper.Map<List<DealModel>>(results);
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, wonders);
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
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

        ///<summary>
        /// Return all rejected Wonders that the user has dis-liked
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("rejectWonders/{userId}")]
        public async Task<HttpResponseMessage> GetRejectWonders(string userId)
        {
            try
            {
                var wonders = await Task.Run(() =>
                {
                    return Mapper.Map<List<DealModel>>(DataContext.AspNetUsers.Where(u => u.Id == userId).SelectMany(w => w.MyRejects));
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
            var listOfCities = await Task.Run(() => { return Mapper.Map<List<CityModel>>(DataContext.Cities.Where(c => !c.Archived)); });
            return Request.CreateResponse(HttpStatusCode.OK, listOfCities);
        }

        /// <summary>
        /// Return Wonder categories
        /// </summary>
        /// <returns></returns>
        [Route("categories")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetCategories()
        {
            var listOfCategories = await Task.Run(() => { return Mapper.Map<List<CategoryModel>>(DataContext.Categories); });
            return Request.CreateResponse(HttpStatusCode.OK, listOfCategories);
        }

        /// <summary>
        /// HTTP GET for genders
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("genders")]
        public async Task<HttpResponseMessage> GetGenders()
        {
            try
            {
                var genders = await Task.Run(() => { return Mapper.Map<List<GenderModel>>(DataContext.Genders); });
                return Request.CreateResponse(HttpStatusCode.OK, genders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
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
        /// Remove a Wonder from "MyWonders" or "MyRejects" collection
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

                    if (user.MyRejects.Contains(deal))
                    {
                        user.MyRejects.Remove(deal);
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


        private IQueryable<Data.Deal> GetNearestWonders(WonderModel model, int mileRadiusFrom, int mileRadiusTo, int amountToTake)
        {
            var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);
            return DataContext.Deals
                           .Where(w => w.Location.Geography.Distance(usersPosition) * .00062 > mileRadiusFrom
                               && w.Location.Geography.Distance(usersPosition) * .00062 <= mileRadiusTo
                               && w.Archived == false
                               && w.Expired != true
                               && _categories.Contains(w.Category.Id)
                               && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                               && w.MyRejectUsers.All(u => u.Id != model.UserId)
                               && w.MyWonderUsers.All(u => u.Id != model.UserId))
                           .OrderBy(x => Guid.NewGuid())
                           .Take(amountToTake);
        }

        private IQueryable<Data.Deal> GetPriorityWonders(WonderModel model)
        {
            return DataContext.Deals
                            .Where(w => w.Priority == true
                                && w.CityId == model.CityId
                                && w.Archived == false
                                && w.Expired != true
                                && _categories.Contains(w.Category.Id)
                                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId));
        }

        private IQueryable<Data.Deal> GetPopularWonders(WonderModel model, int numberToTake)
        {
            return DataContext.Deals
            .Where(w => w.CityId == model.CityId
                && w.Archived == false
                && w.Expired != true
                && _categories.Contains(w.Category.Id)
                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                && w.MyWonderUsers.All(u => u.Id != model.UserId))
            .OrderByDescending(w => w.Likes)
            .Take(numberToTake);
        }

        private IQueryable<Data.Deal> GetRandomWonders(WonderModel model, int numberToTake)
        {
            return DataContext.Deals
                            .Where(w => w.CityId == model.CityId
                                && w.Archived == false
                                && w.Expired != true
                                && _categories.Contains(w.Category.Id)
                                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(numberToTake);
        }

        private void SetUserCategories(string userId)
        {
            var user = DataContext.AspNetUsers.Single(x => x.Id == userId);
            _categories = user.Categories.Select(c => c.Id).ToList();
            if (_categories.Count == 0)
                _categories = DataContext.Categories.Select(c => c.Id).ToList();
        }

    }
}
