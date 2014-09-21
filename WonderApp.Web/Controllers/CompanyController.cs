using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;

namespace WonderApp.Web.Controllers
{
    public class CompanyController : BaseController
    {
        public ActionResult Index()
        {
            var model = Mapper.Map<List<CompanyModel>>(DataContext.Companies.OrderBy(x => x.Name));
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = Mapper.Map<CompanyModel>(DataContext.Companies.Single(x => x.Id == id));
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new CompanyModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CompanyModel model)
        {
            if(model != null && ModelState.IsValid)
            {
                var companyEntity = Mapper.Map<Company>(model);
                DataContext.Companies.Add(companyEntity);

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = Mapper.Map<CompanyModel>(DataContext.Companies.Find(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CompanyModel model)
        {
            if(model != null && ModelState.IsValid)
            {
                var company = DataContext.Deals.Find(model.Id);
                Mapper.Map(model, company);

                return RedirectToAction("Details", new { id = company.Id });
            }
            else
            {
                return View();
            }
        }

    }
}
