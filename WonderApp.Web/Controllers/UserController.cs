using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;
using WonderApp.Web.Models;
using System.Data.Entity;

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
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            DataContext.TurnOffLazyLoading();
            var userViews = new List<UserViewModel>();
            var users = 
                Mapper.Map<List<UserBasicModel>>
                (DataContext
                .AspNetUsers.ToList());

            foreach (var userModel in users)
            {
                var userViewModel = new UserViewModel();
                var user = DataContext.AspNetUsers
                    .AsNoTracking().Include(p => p.UserPreference).Include(w => w.MyWonders)
                    .Single(e => e.Id == userModel.Id);
                var pref = user.UserPreference;

                if (user.CityId != null)
                {
                    var city = DataContext.Cities.SingleOrDefault(c => c.Id == user.CityId);
                    if(city != null) userViewModel.City = city.Name;
                }
                else
                {
                    var cityCounts = new Dictionary<int, int>();
                    if(user.MyWonders.Count() > 0)
                    {                 
                        cityCounts.Add(1, user.MyWonders.Count(w => w.CityId == 1));
                        cityCounts.Add(2, user.MyWonders.Count(w => w.CityId == 2));
                        cityCounts.Add(7, user.MyWonders.Count(w => w.CityId == 7));
                        var cityId = cityCounts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                        var city = DataContext.Cities.SingleOrDefault(c => c.Id == cityId);
                        if(city != null) userViewModel.City = city.Name;
                    }
                    else
                    {
                        userViewModel.City = "No Wonders";
                    }
                }
                
                userViewModel.UserModel = userModel;
                userViewModel.IsAdmin = UserManager.IsInRole(userModel.Id, "Admin");
                userViewModel.OptIn = pref !=null ? pref.EmailMyWonders : false;
                userViews.Add(userViewModel);
            }
            DataContext.TurnOnLazyLoading();
            return View(userViews);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new UserViewModel();
            model.UserModel = new UserBasicModel();
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
                UserBasicModel userModel = model.UserModel;
                var password = "P@ssw0rd1234";

                var user = Mapper.Map<AspNetUser>(userModel);
                
                var newUser = new ApplicationUser
                {
                    UserName = user.Email,
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
            model.UserModel = Mapper.Map<UserBasicModel>(DataContext.AspNetUsers.Find(id));

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
                UserBasicModel userModel = model.UserModel;

                var user = await UserManager.FindByIdAsync(userModel.Id);

                user.UserName = userModel.Email;
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
            
            var model = Mapper.Map<UserBasicModel>(DataContext.AspNetUsers.Find(id));
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(UserBasicModel model)
        {
            //Do not allow deletion of the main accounts
            var adminAccounts = DataContext.AspNetUsers.Where(u =>
                   u.UserName == "mulhall.jason@gmail.com" ||
                   u.UserName == "james.mcclurg@gmail.com" ||
                   u.UserName == "admin@wonderapp.com"
               ).ToList();
            if (adminAccounts.Any(a => a.Id == model.Id))
            {
                AddClientMessage(ClientMessage.Warning, "This is a reserved user which cannot be deleted");
                return View(model);
            }

            if (DataContext.Deals.Any(x => x.Creator_User_Id == model.Id))
            {
                try
                {
                    //Move all Wonders to a default admin user
                    var adminUser = DataContext.AspNetUsers.FirstOrDefault(u => u.UserName.Equals("admin@wonderapp.com"));
                    var wondersToMove = DataContext.Deals.Where(w => w.Creator_User_Id == model.Id);
                    foreach (var deal in wondersToMove)
                    {
                        deal.Creator_User_Id = adminUser.Id;
                    }

                }
                catch (Exception e)
                {
                    //return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "This user has created Wonders - please remove these before attempting to delete this user");
                    AddClientMessage(ClientMessage.Warning, "User has created wonders, so cannot be deleted");
                    return View(model);
                }

               
                
            }

            var user = DataContext.AspNetUsers.FirstOrDefault(u => u.Id == model.Id);
            var userPrefs = DataContext.Preferences.FirstOrDefault(u => u.UserId == model.Id);

            if (userPrefs != null)
            {
                DataContext.Preferences.Remove(userPrefs);
            }

            if (user != null)
            {
                try
                {
                    user.MyRejects.Clear();
                    user.MyWonders.Clear();
                    UserManager.RemoveFromRoles(model.Id, UserManager.GetRoles(model.Id).ToArray());

                    foreach (var login in DataContext.AspNetUserLogins.Where(u => u.UserId == user.Id))
                    { DataContext.AspNetUserLogins.Remove(login); }

                    DataContext.AspNetUsers.Remove(user);
             
                }
                catch (Exception e)
                {
                    AddClientMessage(ClientMessage.Warning, "User cannot be deleted");
                    return View(model);
                }
               
            }

            return RedirectToAction("Index");
        }
    }
}
