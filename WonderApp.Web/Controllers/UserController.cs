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
    public class UserController : BaseController
    {

        public ActionResult Index()
        {
            return View(Mapper.Map<List<UserModel>>(DataContext.AspNetUsers.ToList()));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            try
            {
                var user = Mapper.Map<AspNetUser>(model);
                DataContext.AspNetUsers.Add(user);



                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<UserModel>(DataContext.AspNetUsers.Find(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                var user = DataContext.AspNetUsers.Find((model.Id));
                Mapper.Map(model, user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var model = Mapper.Map<UserModel>(DataContext.AspNetUsers.Find(id));
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(UserModel model)
        {
            if (DataContext.Deals.Any(x => x.Creator_User_Id == model.Id))
            {
                AddClientMessage(ClientMessage.Warning, "User has created wonders, so cannot be deleted");
                return View(model);
            }

            var user = DataContext.AspNetUsers.Find(model.Id);

            DataContext.AspNetUsers.Remove(user);

            return RedirectToAction("Index");
        }
    }
}
