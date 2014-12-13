using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
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
            var userViews = new List<UserViewModel>();
            var users = Mapper.Map<List<UserModel>>(DataContext.AspNetUsers.ToList());

            foreach (var userModel in users)
            {
                var userViewModel = new UserViewModel();
                userViewModel.UserModel = userModel;
                userViewModel.IsAdmin = UserManager.IsInRole(userModel.Id, "Admin");
                userViews.Add(userViewModel);
            }

            return View(userViews);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new UserViewModel();
            model.UserModel = new UserModel();
            model.NewPassword = null;
            model.IsAdmin = false;

            return View(model);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {

            try
            {
                UserModel userModel = model.UserModel;
                var password = "P@ssw0rd1234";

                var user = Mapper.Map<AspNetUser>(userModel);
                
                var newUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                if (model.NewPassword != null)
                {
                    password = model.NewPassword;
                }
               
                var result = await UserManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
              
                    if (model.IsAdmin)
                    {
                         UserManager.AddToRole(newUser.Id, "Admin");
                    }
                   
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
            catch (Exception e)
            {
                
                var errorString = "User not created: \n";
                errorString += e.Message + "\n";

                
                ModelState.AddModelError(string.Empty, errorString);
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var model = new UserViewModel();
            model.UserModel = Mapper.Map<UserModel>(DataContext.AspNetUsers.Find(id));

            model.NewPassword = null;
            model.IsAdmin = UserManager.IsInRole(id, "Admin");

            return View(model);        
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task <ActionResult> Edit(UserViewModel model)
        {
            try
            {
                UserModel userModel = model.UserModel;

                var user = await UserManager.FindByIdAsync(userModel.Id);

                user.UserName = userModel.UserName;
                user.Email = userModel.Email;
                
                var updateUserResult = await UserManager.UpdateAsync(user);
                if (updateUserResult.Succeeded)
                {
                    //Set PW and admin if required
                    if (model.NewPassword != null)
                    {
                        var pwChangeResult = await UserManager.ChangePasswordAsync(model.UserModel.Id, model.OldPassword, model.NewPassword);
                        if (!pwChangeResult.Succeeded)
                        {
                            Debug.Print(pwChangeResult.ToString());
                            var errorString = "Password not updated: \n";
                            foreach (var error in pwChangeResult.Errors)
                            {
                                errorString += error + "\n";
                            }
                            ModelState.AddModelError(string.Empty, errorString);

                            return View();
                        }
                        
                    }

                    if (model.IsAdmin)
                    {
                        if (!UserManager.IsInRole(model.UserModel.Id, "Admin"))
                        {
                            UserManager.AddToRole(model.UserModel.Id, "Admin");
                        }
                        
                    }
                    else
                    {
                        if (UserManager.IsInRole(model.UserModel.Id, "Admin"))
                        {
                            UserManager.RemoveFromRole(model.UserModel.Id, "Admin");
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    Debug.Print(updateUserResult.ToString());
                    var errorString = "User not updated: \n";
                    foreach (var error in updateUserResult.Errors)
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
