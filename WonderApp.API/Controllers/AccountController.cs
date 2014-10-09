﻿using Microsoft.AspNet.Identity;
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
                
                user = new ApplicationUser { UserName = facebookUser.Email, Email = facebookUser.Email, Name = facebookUser.Name };
                
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

    }
}