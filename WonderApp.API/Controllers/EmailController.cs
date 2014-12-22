using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Core.Services;
using WonderApp.Models;

namespace WonderApp.Controllers
{
    /// <summary>
    /// This API handles all email related activities.
    /// </summary>
    [RoutePrefix("api/email")]
    public class EmailController : BaseApiController
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

                    EmailService.SendMyWonderEmails(DataContext);

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
