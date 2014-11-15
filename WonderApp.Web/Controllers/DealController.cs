using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WonderApp.Core.Utilities;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;
using WonderApp.Web.Models;
using Image = WonderApp.Data.Image;
using WonderApp.Core.Services;
using WonderApp.Core.CloudImage;
using Ninject;
using System.IO;
using WonderApp.Models.Extensions;

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
            var model = Mapper.Map<List<DealSummaryModel>>(DataContext.Deals
                .Where(x => (bool) !x.Archived )
                .OrderByDescending(x => x.Id));

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
                Location = new LocationModel(),
                Season = new SeasonModel()
            };

            model.AgesAvailable = Mapper.Map<List<AgeModel>>(DataContext.Ages);
            

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DealCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    var returnModel = CreateDealViewModel<DealCreateModel>();
                    returnModel.DealModel = model.DealModel;
                    returnModel.Image = model.Image;
                    returnModel.TagString = model.TagString;
                    returnModel.AgesAvailable = Mapper.Map<List<AgeModel>>(DataContext.Ages);
           
                    return View(returnModel); 
                }

                var deal = Mapper.Map<Deal>(model.DealModel);

                deal.Address.PostCode = model.DealModel.Location.Name;
                if (model.DealModel.AlwaysAvailable) deal.ExpiryDate = DateTime.Now;

                var tagList = model.TagString.Split(',').ToList();
                foreach (var tag in tagList)
                {
                    int tagId;
                    deal.Tags.Add(int.TryParse(tag, out tagId) ? DataContext.Tags.Find(tagId) : new Tag {Name = tag});
                }

                deal.Ages.Clear();
                foreach (var age in model.AgeString)
                {
                    int ageId;
                    deal.Ages.Add(int.TryParse(age, out ageId)
                        ? DataContext.Ages.First(t => t.Id == ageId)
                        : new Age { Name = age });
                }

                deal.Category = DataContext.Categories.Find(model.DealModel.Category.Id);
                deal.Company = DataContext.Companies.Find(model.DealModel.Company.Id);
                deal.Cost = DataContext.Costs.Find(model.DealModel.Cost.Id);

                var image = CreateImage(model.Image);
                deal.Images.Add(image);

                deal.Creator_User_Id = User.Identity.GetUserId(); ;
                
                DataContext.Deals.Add(deal);
                DataContext.Commit();

                //return RedirectToAction("Index");
                return RedirectToAction("Edit/", new { id = deal.Id, edit = "true" });

            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                return View(model); 
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

        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            
            var dealModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == id));

            var model = CreateDealViewModel<DealEditModel>();
            model.DealModel = dealModel;

            var tagString = "";
            if (dealModel.Tags.Count > 0)
            {
                tagString = dealModel.Tags.Aggregate("", (current, tagModel) => current + (tagModel.Id + ","));
                tagString = tagString.Remove(tagString.Length - 1, 1);
                model.TagString = tagString;
            }

            model.AgesAvailable = Mapper.Map<List<AgeModel>>(DataContext.Ages);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DealEditModel model)
        {
        
            try
            {
                if (!ModelState.IsValid)
                {
                    var dealModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == model.DealModel.Id));
                    var returnModel = CreateDealViewModel<DealEditModel>();
                    returnModel.DealModel = dealModel;

                    var tagString = "";
                    if (dealModel.Tags.Count > 0)
                    {
                        tagString = dealModel.Tags.Aggregate("", (current, tagModel) => current + (tagModel.Id + ","));
                        tagString = tagString.Remove(tagString.Length - 1, 1);
                        returnModel.TagString = tagString;
                    }

                    returnModel.AgesAvailable = Mapper.Map<List<AgeModel>>(DataContext.Ages);
           
                    return View(returnModel);
                }

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

                deal.Ages.Clear();
                foreach (var age in model.AgeString)
                {
                    int ageId;
                    deal.Ages.Add(int.TryParse(age, out ageId)
                        ? DataContext.Ages.First(t => t.Id == ageId)
                        : new Age { Name = age });
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

                //return RedirectToAction("Index");
                return RedirectToAction("Edit/", new { id = model.DealModel.Id, edit = "true" });
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var model = Mapper.Map<DealModel>(DataContext.Deals.Find(id));
            return View(model);
                   
        }

        [HttpPost]
        public ActionResult Delete(DealModel dealModel)
        {
            var deal = DataContext.Deals.Find(dealModel.Id);

            if (User.IsInRole("Admin") ||
                deal.Creator_User_Id != null && User.Identity.GetUserId() == deal.Creator_User_Id)
            {
                deal.Archived = true;
                return RedirectToAction("Index");

            }

            else
            {
                AddClientMessage(ClientMessage.Warning, "Only Administrators and Wonder creator may delete a Wonder");
                return View();
            }

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
                Cities = Mapper.Map<List<CityModel>>(DataContext.Cities).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Seasons = Mapper.Map<List<SeasonModel>>(DataContext.Seasons).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Genders = Mapper.Map<List<GenderModel>>(DataContext.Genders).Select(x =>
                    new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
        }

        [HttpPost]
        public int Copy(int id)
        {
            DealModel dealCreateModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == id));
            Deal deal = Mapper.Map<Deal>(dealCreateModel);

            deal.Title += " COPY";

            var tagList = dealCreateModel.Tags;
            foreach (var tag in tagList)
            {
                deal.Tags.Add(DataContext.Tags.First(t => t.Id == tag.Id));
            }

            deal.Category = DataContext.Categories.First(m => m.Id == dealCreateModel.Category.Id);
            deal.Company = DataContext.Companies.First(m => m.Id == dealCreateModel.Company.Id);
            deal.Cost = DataContext.Costs.First(m => m.Id == dealCreateModel.Cost.Id);
            deal.Creator_User_Id = User.Identity.GetUserId();


            foreach (var image in dealCreateModel.Images)
            {
                Image dataImage = DataContext.Images.First(m => m.Id == image.Id);
                Image newImage = new Data.Image
                {
                    url = dataImage.url,
                    Device = DataContext.Devices.FirstOrDefault(x => x.Type == "iPhone")
                };

                deal.Images.Add(newImage);
            }

            
            DataContext.Deals.Add(deal);
            DataContext.Commit();
            return deal.Id;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns></returns>
        public ActionResult DealDetail(int? dealId)
        {

            var dealModel = Mapper.Map<DealModel>(DataContext.Deals.Single(x => x.Id == dealId));

            return View(dealModel);
        }


        [HttpPut]
        public bool UpdatePriority(int dealId, bool priority)
        {

            try
            {

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
            
        }



    }
}
