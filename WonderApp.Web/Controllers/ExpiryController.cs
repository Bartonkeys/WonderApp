using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WonderApp.Models;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class ExpiryController : BaseController
    {
        public ActionResult Season()
        {
            var model = DataContext.Seasons.ToList().Select(x => new SeasonExpiryViewModel
            {
                Season = new SeasonModel { Id = x.Id, Name = x.Name },
                Expired = x.Deals.Count > 0 && x.Deals.All(y => y.Expired == true)
            });

            return View(model);
        }
    }
}