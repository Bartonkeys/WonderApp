﻿using System.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Models;

namespace WonderApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/app")]
    public class AppController : BaseApiController
    {
        private static string baseUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
        // private static string baseUrl = "https://api.thewonderapp.co/api/";

        [Route("wonders")]
        public async Task<HttpResponseMessage> PostWonders([FromBody]WonderModel model)
        {
            try
            {
                var results = await PostWonders(model, api: "wonder");
                return Request.CreateResponse(HttpStatusCode.Created, results);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("priority")]
        public async Task<HttpResponseMessage> PostPriorityWonders([FromBody]WonderModel model)
        {
            try
            {
                //var results = await PostWonders(model, api: @"wonder/priority");
                var results = await Task.Run(() => GetFor<DealModel>(String.Format("wonder/all/{0}/{1}/{2}", model.UserId, model.CityId, true)));
                return Request.CreateResponse(HttpStatusCode.Created, results);

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
                var results = await PostWonders(model, api: @"wonder/nearest/" + radiusFrom + "/" + radiusTo);
                return Request.CreateResponse(HttpStatusCode.Created, results);

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

                var results = await PostWonders(model, api: String.Format("wonder/popular/{0}", take));
                return Request.CreateResponse(HttpStatusCode.Created, results);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("city")]
        public async Task<HttpResponseMessage> GetCities()
        {
            try
            {
                var results = await Task.Run(() => GetFor<CityModel>("wonder/cities"));
                return Request.CreateResponse(HttpStatusCode.OK, results);
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
                var results = await Task.Run(() => GetFor<DealModel>(String.Format("wonder/all/{0}/{1}/{2}", userId, cityId, priority)));
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("user")]
        public async Task<HttpResponseMessage> GetUsers()
        {
            try
            {
                var results = await Task.Run(() => GetFor<UserInfoModel>("account/users"));
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("myWonders/{userId}")]
        public async Task<HttpResponseMessage> GetMyWonders(string userId)
        {
            try
            {
                var results = await Task.Run(() => GetFor<DealModel>("wonder/myWonders/" + userId));
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("rejectWonders/{userId}")]
        public async Task<HttpResponseMessage> GetRejectWonders(string userId)
        {
            try
            {
                var results = await Task.Run(() => GetFor<DealModel>("wonder/rejectWonders/" + userId));
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("like/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> GetLike(string userId, string wonderId)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await Task.Run(() =>
                    httpClient.PostAsync(baseUrl + "wonder/like/" + userId + "/" + wonderId,
                    new StringContent(String.Empty, Encoding.UTF8, "application/json")));

                if (response.IsSuccessStatusCode)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    throw new Exception(response.ReasonPhrase);


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("dislike/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> GetDisLike(string userId, string wonderId)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await Task.Run(() =>
                    httpClient.PostAsync(baseUrl + "wonder/dislike/" + userId + "/" + wonderId,
                    new StringContent(String.Empty, Encoding.UTF8, "application/json")));

                if (response.IsSuccessStatusCode)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    throw new Exception(response.ReasonPhrase);


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("removeLike/{userId}/{wonderId}")]
        public async Task<HttpResponseMessage> GetRemoveLike(string userId, string wonderId)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await Task.Run(() =>
                    httpClient.PostAsync(baseUrl + "wonder/removeLike/" + userId + "/" + wonderId,
                    new StringContent(String.Empty, Encoding.UTF8, "application/json")));

                if (response.IsSuccessStatusCode)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                    throw new Exception(response.ReasonPhrase);


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        static private async Task<List<T>> GetFor<T>(string api)
        {
            var results = new List<T>();

            var httpClient = new HttpClient();
            var response = await Task.Run(() => httpClient.GetAsync(baseUrl + api));

            using (HttpContent content = response.Content)
            {
                string stringResult = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(stringResult);
            }
        }

        static async private Task<List<DealModel>> PostWonders(WonderModel model, string api)
        {
            var httpClient = new HttpClient();
            var payload = JsonConvert.SerializeObject(model);
            var response = await Task.Run(() => httpClient.PostAsync(baseUrl + api, new StringContent(payload, Encoding.UTF8, "application/json")));

            if (response.IsSuccessStatusCode)
            {
                List<DealModel> results;
                using (HttpContent content = response.Content)
                {
                    var stringResult = await content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<List<DealModel>>(stringResult);
                }

                return results;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }
    }

    public class WonderModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "cityId")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "tagId")]
        public int TagId { get; set; }

    }
}

