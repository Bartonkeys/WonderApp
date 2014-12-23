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

namespace WonderApp.Web.Controllers
{
    public class EmailTemplatesController : BaseController
    {
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
                    var model = Mapper.Map<List<DealModel>>(user.MyWonders);
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