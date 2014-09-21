using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.Models;
using Image = WonderApp.Data.Image;

namespace WonderApp.Web.Controllers
{
    public class DealController : BaseController
    {
        public ActionResult Index()
        {
            var model = Mapper.Map<List<DealModel>>(DataContext.Deals
                .Where(x => !x.Archived )
                .OrderByDescending(x => x.ExpiryDate));

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == id));
            return View(model);
        }

        public ActionResult Create()
        {
            var model = CreateDealViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DealViewModel model)
        {
            try
            {
                var deal = Mapper.Map<Deal>(model.DealModel);

                var tagList = model.TagString.Split(' ').ToList();
                deal.Tags = tagList.Select(x => new Tag {Name = x}).ToList();

                //todo this is shit, sort it out. This is all placeholder bollox
                var image = new Image {url = "placeholder", Device = new Device{Type = "iPhone"}};
                deal.Images.Add(image);
                deal.Category = DataContext.Categories.Find(model.DealModel.Category.Id);
                deal.Company = DataContext.Companies.Find(model.DealModel.Company.Id);
                deal.Cost = DataContext.Costs.Find(model.DealModel.Cost.Id);
                deal.Location = new Location {Latitude = 1, Longitude = 1, Name = "PlaceHolder"};

                DataContext.Deals.Add(deal);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(); 
            }
        }

        public ActionResult Edit(int id)
        {
            var dealModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == id));
            var model = CreateDealViewModel();
            model.DealModel = dealModel;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DealViewModel model)
        {
            try
            {
                var deal = DataContext.Deals.Find(model.DealModel.Id);
                Mapper.Map(model.DealModel, deal);

                return RedirectToAction("Details", new { id = deal.Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var deal = DataContext.Deals.Find(id);
            deal.Archived = true;

            return RedirectToAction("Index");
        }

        private DealViewModel CreateDealViewModel()
        {
            return new DealViewModel
            {
                CostRanges = Mapper.Map<List<CostModel>>(DataContext.Costs).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Range }),
                Categories = Mapper.Map<List<CategoryModel>>(DataContext.Categories).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Locations = Mapper.Map<List<LocationModel>>(DataContext.Locations).Select(x =>
                new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Companies = Mapper.Map<List<CompanyModel>>(DataContext.Companies).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Tags = Mapper.Map<List<TagModel>>(DataContext.Tags).Select(x =>
                new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
        }
    }
}