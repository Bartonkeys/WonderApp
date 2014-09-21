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
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            var model = Mapper.Map<List<CategoryModel>>(DataContext.Categories);
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new CategoryModel());
        }

        [HttpPost]
        public ActionResult Create(CategoryModel model)
        {
            if (model != null & ModelState.IsValid)
            {
                var category = Mapper.Map<Category>(model);
                DataContext.Categories.Add(category);

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = Mapper.Map<CategoryModel>(DataContext.Categories.Find(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var category = DataContext.Categories.Find(model.Id);
                Mapper.Map(model, category);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var category = DataContext.Categories.Find(id);
            DataContext.Categories.Remove(category);

            return RedirectToAction("Index");

        }

    }
}
