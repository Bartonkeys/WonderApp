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
using System.Globalization;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;
using WonderApp.Models.Helpers;

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

        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            
            var model = DataContext.Deals.Where(x => x.Archived == false)
                //.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => new DealSummaryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    IntroDescription = x.IntroDescription,
                    Company = new CompanyModel { Name = x.Company.Name },
                    City = new CityModel { Name = x.City.Name },
                    Category = new CategoryModel { Name = x.Category.Name },
                    Likes = x.Likes,
                    Dislikes = x.MyRejectUsers.Count,
                    Priority = x.Priority.Value,
                    Expired = x.Expired.Value,
                    Season = new SeasonModel{Id = x.Season.Id, Name = x.Season.Name},
                    Broadcast = x.Broadcast != null && x.Broadcast.Value,
                    Tags = x.Tags.Select(y => new TagModel { Name = y.Name })
                });

            ViewBag.isAdmin = User.IsInRole("Admin");
            ViewBag.userId = User.Identity.GetUserId();
            ViewBag.totalLondonWonders= model.Count(c => c.City.Name.Equals("London") && !c.Expired );
            ViewBag.totalNewYorkWonders = model.Count(c => c.City.Name.Equals("New York") && !c.Expired);

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
                Season = new SeasonModel(),
                Ages = new List<AgeModel>
                {
                    DataContext.Ages.Where(a => a.Name.ToLower() == "all").Select(x => new AgeModel{Id = x.Id, Name = x.Name}).Single()
                }
            };

            model.AgesAvailable = DataContext.Ages.Select(x => new AgeModel { Id = x.Id, Name = x.Name }).ToList();
           
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
                    returnModel.AgesAvailable = DataContext.Ages.Select(x => new AgeModel{Id = x.Id, Name = x.Name}).ToList();
           
                    return View(returnModel); 
                }

                var deal = new Deal
                {
                    Title = model.DealModel.Title,
                    Phone = model.DealModel.Phone,
                    Description = model.DealModel.Description,
                    IntroDescription = model.DealModel.IntroDescription,
                    Url = model.DealModel.Url,
                    AlwaysAvailable = model.DealModel.AlwaysAvailable,
                    ExpiryDate = model.DealModel.AlwaysAvailable == true ? DateTime.Now : DateTime.Parse(model.DealModel.ExpiryDate),
                    Likes = model.DealModel.Likes,
                    Archived = model.DealModel.Archived,
                    Expired = model.DealModel.Expired,
                    Location = new Location
                    {
                        Name = model.DealModel.Location.Name,
                        Geography = GeographyHelper.ConvertLatLonToDbGeography(model.DealModel.Location.Longitude.Value, model.DealModel.Location.Latitude.Value)
                    },
                    Priority = model.DealModel.Priority,
                    CityId = model.DealModel.City.Id,
                    Address = new Address
                    {
                        AddressLine1 = model.DealModel.Address.AddressLine1,
                        AddressLine2 = model.DealModel.Address.AddressLine2,
                        PostCode = model.DealModel.Location.Name
                    },
                    Season_Id = model.DealModel.Season.Id,
                    Gender_Id = model.DealModel.Gender.Id
                };

                //var deal = Mapper.Map<Deal>(model.DealModel);

                //deal.Address.PostCode = model.DealModel.Location.Name;
                //if (model.DealModel.AlwaysAvailable) deal.ExpiryDate = DateTime.Now;

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

                return RedirectToAction("Index");
                //return RedirectToAction("Edit/", new { id = deal.Id, edit = "true" });

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

        public ActionResult Edit(int id)
        {
            var deal = DataContext.GetWonder(id);

            if (User.IsInRole("Admin") ||
               deal.Creator_User_Id != null && User.Identity.GetUserId() == deal.Creator_User_Id)
            {

                var dealModel = Mapper.Map<DealModel>(deal);
                dealModel.Tags = Mapper.Map<List<TagModel>>(DataContext.GetWonderTags(dealModel.Id));
                dealModel.Ages = Mapper.Map<List<AgeModel>>(DataContext.GetWonderAges(dealModel.Id));

                //TODO FFS Graham what a hack. Put in SP.
                var cities = DataContext.Cities.ToList();
                var city = cities.Single(c => c.Name == dealModel.City.Name);
                var cityLocation = DataContext.Locations.SingleOrDefault(l => l.Id == city.Id);
                dealModel.City.Location = Mapper.Map<LocationModel>(cityLocation);
                dealModel.Location.Name = DataContext.Locations.Where(l => l.Id == dealModel.Location.Id).Select(x => x.Name).Single();

                if (!dealModel.AlwaysAvailable)
                {
                    dealModel.ExpiryDate = String.IsNullOrEmpty(dealModel.ExpiryDate)
                        ? dealModel.ExpiryDate :
                        (DateTime.ParseExact(dealModel.ExpiryDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.CurrentCulture)).ToString("ddd d MMMM yyyy");
                }

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
            else
            {
                //This should be caught by client side JS
                AddClientMessage(ClientMessage.Warning, "Only Administrators and Wonder creator may edit a Wonder");
                return RedirectToAction("Index");
            }
            
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

                if (User.IsInRole("Admin") ||
                deal.Creator_User_Id != null && User.Identity.GetUserId() == deal.Creator_User_Id)
                {
                    var tagList = !String.IsNullOrEmpty(model.TagString)
                    ? model.TagString.Split(',').ToList() : new List<string>();
                    deal.Tags.Clear();
                    foreach (var tag in tagList)
                    {
                        int tagId;
                        deal.Tags.Add(int.TryParse(tag, out tagId)
                            ? DataContext.Tags.First(t => t.Id == tagId)
                            : new Tag { Name = tag });
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

                    deal.Address.PostCode = model.DealModel.Location.Name;

                    return RedirectToAction("Index");
                    //return RedirectToAction("Edit/", new { id = model.DealModel.Id, edit = "true" });

                }

                else
                {
                    AddClientMessage(ClientMessage.Warning, "Only Administrators and Wonder creator may edit a Wonder");

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

                
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
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
                CostRanges = DataContext.Costs.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Range }),
                Categories = DataContext.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Locations = DataContext.Locations.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Companies = DataContext.Companies.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Cities = DataContext.Cities.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Seasons = DataContext.Seasons.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }),
                Genders = DataContext.Genders.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
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

            var dealModel = DataContext.Deals.Where(x => x.Id == dealId)
                .AsNoTracking()
                .Select(x => new DealModel
                {
                    Id = x.Id,
                    IntroDescription = x.IntroDescription,
                    Description = x.Description,
                    Cost = new CostModel { Range = x.Cost.Range },
                    Likes = x.Likes,
                    Location = new LocationModel { Name = x.Location.Name },
                    Category = new CategoryModel { Name = x.Category.Name },
                    Tags = x.Tags.Select(t => new TagModel { Id = t.Id, Name = t.Name }).ToList(),
                    AlwaysAvailable = x.AlwaysAvailable ?? false,
                    ExpiryDate = x.ExpiryDate.ToString(),
                    Images = new List<ImageModel> { new ImageModel { Id = x.Images.FirstOrDefault().Id, url = x.Images.FirstOrDefault().url} }
                }).Single();

            //var dealModel =  Mapper.Map<DealModel>(DataContext.GetWonder(dealId.Value));
            //var dealModel = DataContext.Deals.Where(x => x.Id == dealId).Project().To<DealModel>().Single();

            if (dealModel.AlwaysAvailable) dealModel.ExpiryDate = "Never";

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
