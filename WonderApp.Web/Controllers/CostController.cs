using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;

namespace WonderApp.Web.Controllers
{
    public class CostController : BaseController
    {
        
        public ActionResult Index()
        {
            return View(Mapper.Map<List<CostModel>>(DataContext.Costs.ToList()));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(CostModel model)
        {
            try
            {
                var cost = Mapper.Map<Cost>(model);
                DataContext.Costs.Add(cost);

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<CostModel>(DataContext.Costs.Find(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(CostModel model)
        {
            try
            {
                var cost = DataContext.Costs.Find((model.Id));
                Mapper.Map(model, cost);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var model = Mapper.Map<CostModel>(DataContext.Costs.Find(id));
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(CostModel model)
        {
            if (DataContext.Deals.Any(x => x.Cost.Id == model.Id))
            {
                AddClientMessage(ClientMessage.Warning, "Range is in use, so cannot be deleted");
                return View(model);
            }

            var cost = DataContext.Costs.Find(model.Id);

            DataContext.Costs.Remove(cost);

            return RedirectToAction("Index");
        }
    }
}
