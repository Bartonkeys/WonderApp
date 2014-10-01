using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WonderApp.Core.CloudImage;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class ImageController : BaseController
    {
        protected CloudImageService CloudImageService;
        static readonly string ServerUploadFolder = Path.GetTempPath();

        [Inject]
        public ImageController(ICloudImageProvider cloudImageProvider)
        {
            CloudImageService = new CloudImageService(cloudImageProvider);
        }

        public ActionResult DealUpload(int id)
        {
            var model = new ImageViewModel
            {
                DealId = id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DealUpload(ImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var image = Image.FromStream(model.Image.InputStream, true, true);
                var imageName = Path.GetFileName(model.Image.FileName);
                var imageUrl = CloudImageService.SaveImageToCloud(image, imageName);

                var deal = DataContext.Deals.Find(model.DealId);
                deal.Images.Add(new Data.Image
                {
                    url = imageUrl,
                    Device = DataContext.Devices.FirstOrDefault(x => x.Type == "iPhone")
                });

                return RedirectToAction("Index", "Deal");
            }

            return View(model);
        }

    }
}
