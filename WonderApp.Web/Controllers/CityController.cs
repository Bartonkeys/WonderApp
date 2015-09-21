using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class CityController : BaseController
    {
        public ActionResult Index()
        {
            var model = Mapper.Map<List<CityModel>>(DataContext.Cities.OrderBy(x => x.Name));
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = Mapper.Map<CityModel>(DataContext.Cities.Single(x => x.Id == id));
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new CityViewModel
            {
                CityModel = new CityModel(),
                
            };
            return View(model);

        }

        [HttpPost]
        public ActionResult Create(CityViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                
                var cityEntity = Mapper.Map<City>(model.CityModel);
                DataContext.Cities.Add(cityEntity);
   
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = new CityViewModel
            {
                CityModel = Mapper.Map<CityModel>(DataContext.Cities.Find(id)),
                
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CityViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var city = DataContext.Cities.Find(model.CityModel.Id);
                Mapper.Map(model.CityModel, city);

                return RedirectToAction("Details", new { id = city.Id });
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var model = Mapper.Map<CityModel>(DataContext.Cities.Find(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(CityModel model)
        {
            if (DataContext.Deals.Any(x => x.City.Id == model.Id))
            {
                var dealsWithCity = DataContext.Deals.Where(x => x.City.Id == model.Id);
                string dealString = "Deals which link to this city:\n";
                foreach (var deal in dealsWithCity)
                {
                    dealString += deal.Title + "\n";
                }

                AddClientMessage(ClientMessage.Warning, "City is in use, so cannot be deleted.\n" + dealString);
                return View(model);
            }

            var city = DataContext.Cities.Find(model.Id);

            DataContext.Cities.Remove(city);

            return RedirectToAction("Index");
        }

    }
}
