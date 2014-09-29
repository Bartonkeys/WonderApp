using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using WonderApp.Core.Services;
using WonderApp.Core.CloudImage;
using Ninject;
using System.IO;

namespace WonderApp.Web.Controllers
{
    public class DealController : BaseController
    {
        protected CloudImageService CloudImageService;

        [Inject]
        public DealController(ICloudImageProvider cloudImageProvider)
        {
            CloudImageService = new CloudImageService(cloudImageProvider);
        }

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
            var model = CreateDealViewModel<DealCreateModel>();
            model.DealModel = new DealModel
            {
                Category = new CategoryModel(),
                Company = new CompanyModel(),
                Cost = new CostModel(),
                Location = new LocationModel()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DealCreateModel model)
        {
            try
            {
                var deal = Mapper.Map<Deal>(model.DealModel);

                var tagList = model.TagString.Split(',').ToList();
                foreach (var tag in tagList)
                {
                    int tagId;
                    deal.Tags.Add(int.TryParse(tag, out tagId) ? DataContext.Tags.Find(tagId) : new Tag {Name = tag});
                }

                deal.Category = DataContext.Categories.Find(model.DealModel.Category.Id);
                deal.Company = DataContext.Companies.Find(model.DealModel.Company.Id);
                deal.Cost = DataContext.Costs.Find(model.DealModel.Cost.Id);

                var image = CreateImage(model.Image);
                deal.Images.Add(image);

                DataContext.Deals.Add(deal);
                DataContext.Commit();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(); 
            }
        }

        private Data.Image CreateImage(HttpPostedFileBase httpPostedFileBase)
        {
            var image = System.Drawing.Image.FromStream(httpPostedFileBase.InputStream, true, true);
            var imageName = Path.GetFileName(httpPostedFileBase.FileName);
            var imageUrl = CloudImageService.SaveImageToCloud(image, imageName);

            return new Data.Image
                {
                    url = imageUrl,
                    Device = DataContext.Devices.FirstOrDefault(x => x.Type == "iPhone")
                };
        }

        public ActionResult Edit(int id)
        {

            var dealModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == id));
            var model = CreateDealViewModel<DealEditModel>();
            model.DealModel = dealModel;

            var tagString = dealModel.Tags.Aggregate("", (current, tagModel) => current + (tagModel.Id + ","));
            tagString = tagString.Remove(tagString.Length - 1, 1);
            model.TagString = tagString;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DealEditModel model)
        {
        
            try
            {
                var deal = DataContext.Deals.Find(model.DealModel.Id);

                var tagList = !String.IsNullOrEmpty(model.TagString) 
                    ? model.TagString.Split(',').ToList() : new List<string>();
                deal.Tags.Clear();
                foreach (var tag in tagList)
                {
                    int tagId;
                    deal.Tags.Add(int.TryParse(tag, out tagId)
                        ? DataContext.Tags.First(t => t.Id == tagId)
                        : new Tag {Name = tag});
                }

                //todo Can the AuotMapper config handle this somehow?
                deal.Category = DataContext.Categories.First(m => m.Id == model.DealModel.Category.Id);
                deal.Company = DataContext.Companies.First(m => m.Id == model.DealModel.Company.Id);
                deal.Cost = DataContext.Costs.First(m => m.Id == model.DealModel.Cost.Id);

                if (model.Image != null)
                {
                    var image = CreateImage(model.Image);

                    var oldImage = deal.Images.First();                   
                    deal.Images.Clear();
                    DataContext.Images.Remove(oldImage);
                    deal.Images.Add(image);
                }

                Mapper.Map(model.DealModel, deal);

                return RedirectToAction("Index");
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

        private T CreateDealViewModel<T>() where T: DealViewModel, new()
        {
            return new T
            {
                CostRanges = Mapper.Map<List<CostModel>>(DataContext.Costs).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Range }),
                Categories = Mapper.Map<List<CategoryModel>>(DataContext.Categories).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Locations = Mapper.Map<List<LocationModel>>(DataContext.Locations).Select(x =>
                new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Companies = Mapper.Map<List<CompanyModel>>(DataContext.Companies).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
            };
        }


    }
}
