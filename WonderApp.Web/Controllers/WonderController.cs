using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WonderApp.Models;

namespace WonderApp.Web.Controllers
{
    [RoutePrefix("api/wonder")]
    public class WonderController : BaseApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetWonders()
        {
            var model = Mapper.Map<List<DealModel>>(DataContext.Deals
                .Where(x => !x.Archived)
                .OrderByDescending(x => x.ExpiryDate));

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
