using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class DealController : BaseController
    {
        // GET: Deal
        public ActionResult Create()
        {
            var model = new DealViewModel
            {
                CostRanges = Mapper.Map<List<CostModel>>(DataContext.Costs).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Range}),
                Categories = Mapper.Map<List<CategoryModel>>(DataContext.Categories).Select(x =>
                    new SelectListItem {Value = x.Id.ToString(),Text = x.Name})
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DealViewModel model)
        {
            var deal = Mapper.Map<Deal>(model.DealModel);
            DataContext.Deals.Add(deal);
            return View();
        }
    }
}