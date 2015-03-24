using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WonderApp.Models;
using System.Net;
using WonderApp.Constants;
using AutoMapper;
using WonderApp.Data;

namespace WonderApp.Controllers
{
    /// <summary>
    /// This API handles all account related activities.
    /// </summary>
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Verify Facebook access token.
        /// If not authorised with Facebook fire back a 403.
        /// If authorised and already registered then return 200 with userId.
        /// If authorised and not registered, then register and return 201 with userId
        /// If any other error, return 500 with message.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Facebook/{accessToken}")]
        public async Task<HttpResponseMessage> GetFacebook(string accessToken)
        {
            try
            {
                var response = await GetFacebookResponse(accessToken);

                if (!response.IsSuccessStatusCode) 
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                var content = await response.Content.ReadAsStringAsync();
                var facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUserModel>(content);

                var user = await UserManager.FindByEmailAsync(facebookUser.Email);
                if (user != null)
                {
                    var logins = await UserManager.GetLoginsAsync(user.Id);
                    foreach (var login in logins)
                    {
                        if(login.ProviderKey == facebookUser.ID)
                            return Request.CreateResponse(HttpStatusCode.OK, user.Id);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WonderAppConstants.UserAlreadyRegisted);
                }
                
                user = new ApplicationUser
                {
                    UserName = facebookUser.Email, 
                    Email = facebookUser.Email, 
                    Name = facebookUser.Name,
                    Forename = facebookUser.FirstName,
                    Surname = facebookUser.LastName
                    
                };
                
                var result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WonderAppConstants.CreateUserError);

                var userLoginInfo = new UserLoginInfo(WonderAppConstants.Facebook, facebookUser.ID);

                result = await UserManager.AddLoginAsync(user.Id, userLoginInfo);
                if (!result.Succeeded)
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, WonderAppConstants.CreateFacebookDetailsError);

                return Request.CreateResponse(HttpStatusCode.Created, user.Id);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        /// <summary>
        /// Get All Users as a List of UserModel objects
        /// If any error, return 500 with message.
        /// </summary>
        /// 
        /// <returns>HttpResponseMessage</returns>
        [AllowAnonymous]
        [Route("Users")]
        public HttpResponseMessage GetAllUsers()
        {
            try
            {
                var users = Mapper.Map<List<UserModel>>(DataContext.AspNetUsers);
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        private async Task<HttpResponseMessage> GetFacebookResponse(string accessToken)
        {
            var path = WonderAppConstants.FacebookTokenUrl + accessToken;
            var client = new HttpClient();
            var uri = new Uri(path);
            return await client.GetAsync(uri);
        }

        private void PopulateFacebookEmailWithDummyData(FacebookUserModel facebookUser)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomUserName = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            facebookUser.Email = facebookUser.Email ?? String.Format("{0}@test.com", randomUserName);
        }

        /// <summary>
        /// HTTP POST to save personal information.
        /// Returns HTTP StatusCode 200.
        /// If error, return Http Status Code 500 with error message.
        /// </summary>
        /// <returns></returns>   
        [Route("personal")]
        public async Task<HttpResponseMessage> PostPersonal([FromBody]UserInfoModel userPersonal)
        {
            try
            {
                var user = DataContext.AspNetUsers.Find(userPersonal.Id);

                if (user == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("That user does not exist: {0}", userPersonal.Id));
                }

                user.Gender = DataContext.Genders.FirstOrDefault(g => g.Id == userPersonal.Gender.Id);
                var categories = new List<Data.Category>();
                if (userPersonal.MyCategories != null && userPersonal.MyCategories.Any())
                {
                    foreach (CategoryModel cat in userPersonal.MyCategories)
                    {
                        categories.Add(DataContext.Categories.FirstOrDefault(c => c.Id == cat.Id));
                    }
                }
                user.Categories.Clear();
                user.Categories = categories;
               
                if (user.UserPreference == null)
                {
                    user.UserPreference = new Data.UserPreference();
                }
                user.UserPreference.Reminder = DataContext.Reminders.FirstOrDefault(r => r.Id == userPersonal.UserPreference.Reminder.Id);
                user.UserPreference.EmailMyWonders = userPersonal.UserPreference.EmailMyWonders;

                Mapper.Map(userPersonal, user);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// HTTP POST to save user's preferred categories
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("categories")]
        public async Task<HttpResponseMessage> PostCategories([FromBody]UserCategoryModel model)
        {
            try
            {
                var response = await Task.Run(() =>
                {
                    var user = DataContext.AspNetUsers.Find(model.UserId);

                    if (user == null)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("That user does not exist: {0}", model.UserId));

                    var categories = new List<Data.Category>();
                    if (model.MyCategories != null)
                    {
                        foreach (CategoryModel categoryModel in model.MyCategories)
                        {
                            var category = DataContext.Categories.SingleOrDefault(c => c.Id == categoryModel.Id);
                            if (category != null) categories.Add(category);
                        }
                    }

                    user.Categories.Clear();
                    categories.ForEach(x => user.Categories.Add(x));
                    DataContext.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK);
                });

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// HTTP POST to save user's email preferences
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("email")]
        public async Task<HttpResponseMessage> PostEmailPreferences([FromBody]UserEmailModel model)
        {
            try
            {
                var response = await Task.Run(() =>
                {
                    var user = DataContext.AspNetUsers.Find(model.UserId);

                    if (user == null)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("That user does not exist: {0}", model.UserId));

                    if (user.UserPreference == null) user.UserPreference = new Data.UserPreference();
                    user.UserPreference.EmailMyWonders = model.EmailMyWonders;
                    DataContext.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK);
                });

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// HTTP POST to save user's gender
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("gender")]
        public async Task<HttpResponseMessage> PostGender([FromBody]UserGenderModel model)
        {
            try
            {
                var response = await Task.Run(() =>
                {
                    var user = DataContext.AspNetUsers.Find(model.UserId);

                    if (user == null)
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("That user does not exist: {0}", model.UserId));

                    if (model.Gender != null)
                    {
                        user.Gender = DataContext.Genders.FirstOrDefault(g => g.Id == model.Gender.Id);
                        DataContext.Commit();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                });

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Return all User preferences/personal data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("personal/{userId}")]
        public async Task<HttpResponseMessage> GetUserInfo(string userId)
        {
            try
            {
                var user = await Task.Run(() =>
                {
                    AspNetUser aspNetUser = DataContext.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault();
                    List<CategoryModel> categories = Mapper.Map<List<CategoryModel>>(DataContext.AspNetUsers.Find(userId).Categories);

                    UserInfoModel userInfo = Mapper.Map<UserInfoModel>(DataContext.AspNetUsers.Find(userId));
                    userInfo.MyCategories = categories;
                    return userInfo;
                });

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Completely remove user, delete the lot; user detail and wonder likes / dislikes
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("delete/{userId}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string userId)
        {
            try
            {
                var aspNetUser = DataContext.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault();

                if (aspNetUser != null)
                {
                    aspNetUser.MyRejects.Clear();
                    aspNetUser.MyWonders.Clear();
                    DataContext.Commit();

                    if (DataContext.Deals.Count(w => w.Creator_User_Id == aspNetUser.Id) > 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "This user has created Wonders - please remove these before attempting to delete this user");
                    }

                    DataContext.AspNetUsers.Remove(aspNetUser);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "This user does not exist");
                    
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}