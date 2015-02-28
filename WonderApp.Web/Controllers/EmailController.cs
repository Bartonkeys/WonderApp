using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using WonderApp.Models;
using WonderApp.Web.InfaStructure;
using WonderApp.Web.Models;

namespace WonderApp.Web.Controllers
{
    public class EmailTemplatesController : BaseController
    {
        private const int NumberOfWonders = 10;

        // GET: Email
        public ActionResult Index()
        {
            var model = DataContext.AspNetUsers;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>   
        [System.Web.Http.Route("MyWondersEmail")]
        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public ActionResult MyWondersEmailTemplate(string id)
        {
                
            try
            {
                var user = DataContext.AspNetUsers.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var amountToSkip = user.MyWonders.Count <= NumberOfWonders ? 0 : user.MyWonders.Count - NumberOfWonders;
                    var recentWonders = user.MyWonders.Where(x => x.Archived != true).Skip(amountToSkip).Reverse();
                    //var recentWonders = user.MyWonders.Skip(user.MyWonders.Count - NumberOfWonders);
          
                    
                    var model = new EmailTemplateViewModel();
                    model.User = Mapper.Map<UserModel>(user);
                    model.Wonders = Mapper.Map<List<DealModel>>(recentWonders);

                    //TODO: move these to config properties
                    model.UrlString = "https://cms.thewonderapp.co/content/images/";
                    model.UnsubscribeLink = "mailto:unsubscribe@thewonderapp.co";

                    return View(model);
                }
                return View();           
            }

            catch (Exception ex)
            {
                AddClientMessage(ClientMessage.Warning, ex.Message);
                return View();     
            }

        }
    }
}