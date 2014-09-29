using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using WonderApp.Contracts.DataContext;
using WonderApp.Web.InfaStructure;

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

        /// <summary>
        ///     Add message to show to the user via bootstrap allert
        /// </summary>
        /// <param name="type">Message Type (use static properties of the <see cref="ClientMessage" /> type)</param>
        /// <param name="text">Message Text</param>
        protected void AddClientMessage(string type, string text)
        {
            var messages = ViewBag.ClientMessages as List<ClientMessage>;
            if (messages == null)
            {
                messages = new List<ClientMessage>();
                ViewBag.ClientMessages = messages;
            }

            messages.Add(new ClientMessage { Type = type, Text = text });
        }
    }
}