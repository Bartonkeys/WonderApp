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

                TempData["category"] = category;

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
            var model = Mapper.Map<CategoryModel>(DataContext.Categories.Find(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(CategoryModel model)
        {
            if (DataContext.Deals.Any(x => x.Category.Id == model.Id))
            {
                AddClientMessage(ClientMessage.Warning, "Category is in use, so cannot be deleted");
                return View(model);
            }

            var category = DataContext.Categories.Find(model.Id);

            DataContext.Categories.Remove(category);

            return RedirectToAction("Index");
        }

    }
}
