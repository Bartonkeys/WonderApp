using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;

namespace WonderApp.Web.Controllers
{
    public class CostController : BaseController
    {

        public ActionResult Index()
        {
            return View(Mapper.Map<List<CostModel>>(DataContext.Costs.ToList()));
        }

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<CostModel>(DataContext.Costs.Find(id)));
        }

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

        public ActionResult Delete(int id)
        {
            var cost = DataContext.Costs.Find(id);
            DataContext.Costs.Remove(cost);

            return RedirectToAction("Index");
        }
    }
}
