using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WonderApp.Web.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        // GET: App
        public ActionResult Index()
        {
            return View();
        }
    }
}