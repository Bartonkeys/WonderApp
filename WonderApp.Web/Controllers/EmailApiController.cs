﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    [RoutePrefix("api/email")]
    public class EmailApiController : BaseApiController
    {
        /// <summary>
        /// HTTP POST to send emails
        /// Returns HTTP StatusCode 200.
        /// If error, return Http Status Code 500 with error message.
        /// </summary>
        /// <returns></returns>   
        [Route("MyWonders")]
        public async Task<HttpResponseMessage> PostEmailMyWonders([FromBody]AuthModel authModel)
        {
            try
            {
                if (authModel.Password.Equals("SendMyWonderEmails"))
                {

                    //Check time of last send
                    var oneweekAgo = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0));

                    if (DataContext.NotificationEmails.Any() && DataContext.NotificationEmails.Any(e => e.Sent > oneweekAgo))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "No need to run yet");
                    }

                    //TODO - inject dependent on email provider
                    var emailService = new Core.Services.EmailService();

                    await emailService.SendMyWonderEmails(DataContext);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You are not authorized to execute this command");
                }
            }


            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }
    }
}
