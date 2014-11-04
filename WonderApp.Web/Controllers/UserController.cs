using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UserController()
        {

        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }
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
        public async Task<ActionResult> Create(UserModel model)
        {
            try
            {
                var user = Mapper.Map<AspNetUser>(model);
                
                var newUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };
               
                var result = await UserManager.CreateAsync(newUser, "Y)rma1234");
                if (result.Succeeded)
                {
                   
                }
                else
                {
                    Debug.Print(result.ToString());
                }

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
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
        public ActionResult Delete(string id)
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
