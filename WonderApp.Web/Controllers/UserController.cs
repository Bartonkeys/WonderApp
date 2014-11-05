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
                    UserName = user.UserName,
                    Email = user.Email
                };
               
                var result = await UserManager.CreateAsync(newUser, "P@ssw0rd1234");
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Debug.Print(result.ToString());
                    var errorString = "User not created: \n";
                    foreach(var error in result.Errors)
                    {
                       errorString += error + "\n";
                    }
                    ModelState.AddModelError(string.Empty, errorString);

                    return View();
                }

               
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
        public async Task <ActionResult> Edit(UserModel model)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(model.Id);

                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Debug.Print(result.ToString());
                    var errorString = "User not updated: \n";
                    foreach (var error in result.Errors)
                    {
                        errorString += error + "\n";
                    }
                    ModelState.AddModelError(string.Empty, errorString);

                    return View();
                }


                return RedirectToAction("Index");
            }
            catch (Exception e)
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
