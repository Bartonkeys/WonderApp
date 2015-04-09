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
using System.Data.Entity.Spatial;

namespace WonderApp.Controllers
{
    /// <summary>
    /// The main API for communication with device
    /// </summary>
    [RoutePrefix("api/wonder")]
    public class WonderController : BaseApiController
    {
        private List<int> _categories;
        private List<int> _genders;
        private List<DealModel> _wonders; 
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

                var wonders = new List<DealModel>();

                wonders = await Task.Run(() => GetWonders(model.UserId, model.CityId, priority: true));

                if (wonders.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, wonders.ToList());

                _wonders = await Task.Run(() => GetWonders(model.UserId, model.CityId, priority: false));

                if (model.Latitude != null && model.Longitude != null)
                {
                    wonders = await Task.Run(() =>
                    {
                        if (_wonders.Count <= WonderAppConstants.DefaultMaxNumberOfWonders)
                            return _wonders.ToList();

                        var usersPosition = GeographyHelper.ConvertLatLonToDbGeography(model.Longitude.Value, model.Latitude.Value);

                        var popularWonders = GetPopularWonders(WonderAppConstants.DefaultNumberOfWondersToTake);
                        var randomWonders = GetRandomWonders(WonderAppConstants.DefaultNumberOfWondersToTake);
                        var oneMileWonders = GetNearestWonders(usersPosition, from: 0, to: 1);
                        var threeMileWonders = GetNearestWonders(usersPosition, from: 1, to: 3);

                        var results = oneMileWonders.Union(threeMileWonders).Union(popularWonders).Union(randomWonders);
                        results = results.OrderBy(x => Guid.NewGuid());

                        return results.ToList();

                    });
                }
                else
                {
                    wonders = await Task.Run(() =>
                    {
                        var popularWonders = GetPopularWonders(WonderAppConstants.DefaultNumberOfWondersToTake *2);
                        var randomWonders = GetRandomWonders(WonderAppConstants.DefaultNumberOfWondersToTake *2);

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
                SetUserGenders(model.UserId);

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
                SetUserGenders(model.UserId);

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
                SetUserGenders(model.UserId);

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

        [Route("all/{userId}/{cityId}/{priority}")]
        public async Task<HttpResponseMessage> GetAllWonders(string userId, int cityId, bool priority)
        {
            try
            {
                    var wonders = await Task.Run(() => GetWonders(userId, cityId, priority));
                    return Request.CreateResponse(HttpStatusCode.OK, wonders);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private List<DealModel> GetWonders(string userId, int cityId, bool priority)
        {

            var results = DataContext.GetWonders(userId, cityId, priority);
            var wonderModels = Mapper.Map<List<DealModel>>(results);
            var tags = DataContext.GetTags(userId, cityId, priority);
            var ages = DataContext.GetAges(userId, cityId, priority);
            var cities = DataContext.Cities.ToList();

            wonderModels.ForEach(w =>
            {
                var city = cities.Single(c => c.Name == w.City.Name);
                w.City.Location = Mapper.Map<LocationModel>(city.Location);
                w.Tags = Mapper.Map<List<TagModel>>(tags.Where(t => t.DealId == w.Id));
                w.Ages = Mapper.Map<List<AgeModel>>(ages.Where(t => t.DealId == w.Id));
            });

            return wonderModels;
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
                    var myWonders = DataContext.GetMyWonders(userId);
                    return Mapper.Map<List<DealModel>>(myWonders.ToList());
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
                               && _genders.Contains(w.Gender.Id)
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
                                && _genders.Contains(w.Gender.Id)
                                && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)
                                && w.MyRejectUsers.All(u => u.Id != model.UserId)
                                && w.MyWonderUsers.All(u => u.Id != model.UserId));
        }

        private IEnumerable<DealModel> GetPopularWonders(int numberToTake)
        {
            return _wonders.OrderByDescending(w => w.Likes).Take(numberToTake);
        }

        private IEnumerable<DealModel> GetRandomWonders(int numberToTake)
        {
            return _wonders.OrderBy(x => Guid.NewGuid()).Take(numberToTake);
        }

        private IEnumerable<DealModel> GetNearestWonders(DbGeography usersPosition, int from, int to)
        {
            return _wonders.Where(w =>
            {
                var wonderLocation =
                    GeographyHelper.ConvertLatLonToDbGeography(w.Location.Longitude.Value, w.Location.Latitude.Value);

                return wonderLocation.Distance(usersPosition) * .00062 > from &&
                       wonderLocation.Distance(usersPosition) * .00062 <= to;
            }).OrderBy(x => Guid.NewGuid()).Take(WonderAppConstants.DefaultNumberOfWondersToTake);
        }

        private IQueryable<Data.Deal> GetPopularWonders(WonderModel model, int numberToTake)
        {
            return DataContext.Deals
            .Where(w => w.CityId == model.CityId
                && w.Archived == false
                && w.Expired != true
                && _categories.Contains(w.Category.Id)
                && _genders.Contains(w.Gender.Id)
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
                                && _genders.Contains(w.Gender.Id)
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

        private void SetUserGenders(string userId)
        {
            var user = DataContext.AspNetUsers.Single(x => x.Id == userId);
            _genders = new List<int>();
            _genders.Add(DataContext.Genders.Single(g => g.Name == "All").Id);

            if (user.Gender != null)
                _genders.Add(user.Gender.Id);
            else
                _genders.AddRange((DataContext.Genders.Where(g => g.Name != "All").Select(x => x.Id).ToList()));
                
        }
    }
}
