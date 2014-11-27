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
            var model = Mapper.Map<List<SeasonModel>>(DataContext.Seasons)
                .Select(x => new SeasonExpiryViewModel
                {
                    Season = x,
                    Expired = x.Deals.All(y => y.Expired == true)
                });

            return View(model);
        }
    }
}