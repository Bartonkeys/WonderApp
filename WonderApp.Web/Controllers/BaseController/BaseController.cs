using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using WonderApp.Contracts.DataContext;

namespace WonderApp.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        [Inject]
        public IDataContext DataContext { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            try
            {
                DataContext.Commit();
            }
            catch (Exception exc)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
            }

        }
    }
}