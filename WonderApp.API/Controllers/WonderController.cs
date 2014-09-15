using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using WonderApp.Models;

namespace WonderApp.Controllers
{
    /// <summary>
    /// The main API for communication with device
    /// </summary>
    public class WonderController : BaseApiController
    {
        /// <summary>
        /// Return all deals
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetDeals()
        {
            var deals = Mapper.Map<List<DealModel>>(DataContext.Deals);
            return Request.CreateResponse(HttpStatusCode.OK, deals);
        }
    }
}
