using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WonderApp.Core.CloudImage;

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

        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        // GET: Image/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Image/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Image/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Image/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Image/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Image
        public ActionResult DealUpload(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase photo, string id)
        {
            string path = @"C:\Temp\";

            if (photo != null)
            {
                String fullPath = path + photo.FileName;
                photo.SaveAs(fullPath);
                CloudImageService.SaveImageToCloud(fullPath, photo.FileName);
                
            }

            return RedirectToAction("Index");
        }

    }
}
