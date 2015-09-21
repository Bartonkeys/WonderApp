using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Ninject;
using WonderApp.Contracts.DataContext;

namespace WonderApp.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        [Inject]
        public IDataContext DataContext { get; set; }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
             var response = base.ExecuteAsync(controllerContext, cancellationToken);
            try
            {
                DataContext.Commit();
            }
            catch (Exception exc)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
            }
            return response;
        }
    }
}