using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using WonderApp.Data;

namespace WonderApp.Web.Controllers
{
    [RoutePrefix("api/tag")]
    public class TagController : BaseApiController
    {

        private List<SelectItem> _tags = new List<SelectItem>();
        private List<SelectItem> _companies = new List<SelectItem>();

        //TODO: hack for testing if DB read fails 
        private readonly List<SelectItem> _tagsLocal = new List<SelectItem>{
            new SelectItem {id = 1, text = "food"},
            new SelectItem {id = 2, text = "drink"},
            new SelectItem {id = 3, text = "shopping"},
            new SelectItem {id = 4, text = "entertainment"}  
        };

        //TODO: hack for testing if DB read fails 
        private readonly List<SelectItem> _companiesLocal = new List<SelectItem>{
            new SelectItem {id = 1, text = "Acme Insurance"},
            new SelectItem {id = 2, text = "Tesco"},
            new SelectItem {id = 3, text = "IBM"},
            new SelectItem {id = 4, text = "Apple"}  
        };

        [HttpGet]
        public IEnumerable<SelectItem> SearchTag(string id)
        {
            try
            {
                var tagsFromDb = DataContext.Tags;
                foreach (var tag in tagsFromDb)
                {
                    var si = new SelectItem();
                    si.id = tag.Id;
                    si.text = tag.Name;
                    _tags.Add(si);
                }
            }

            catch (Exception e)
            {
                _tags = _tagsLocal;
            }
            
           
            var query = _tags.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return query;
        }

        [HttpGet]
        public IEnumerable<SelectItem> GetTag(string id)
        {
            try
            {
                var tagsFromDb = DataContext.Tags;
                foreach (var tag in tagsFromDb)
                {
                    var si = new SelectItem();
                    si.id = tag.Id;
                    si.text = tag.Name;
                    _tags.Add(si);
                }
            }

            catch (Exception e)
            {
                _tags = _tagsLocal;
            }

            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] {','});
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_tags.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }


        [HttpGet]
        public IEnumerable<SelectItem> SearchCompany(string id)
        {
            try
            {
                var companiesFromDb = DataContext.Companies;
                foreach (var company in companiesFromDb)
                {
                    var si = new SelectItem();
                    si.id = company.Id;
                    si.text = company.Name;
                    _companies.Add(si);
                }
            }

            catch (Exception e)
            {
                _companies = _companiesLocal;
            }


            var query = _companies.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return query;
        }

        [HttpGet]
        public IEnumerable<SelectItem> GetCompany(string id)
        {
            try
            {
                var companiesFromDb = DataContext.Companies;
                foreach (var company in companiesFromDb)
                {
                    var si = new SelectItem();
                    si.id = company.Id;
                    si.text = company.Name;
                    _companies.Add(si);
                }
            }

            catch (Exception e)
            {
                _companies = _companiesLocal;
            }

            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    items.Add(_companies.FirstOrDefault(m => m.id == idInt));
                }
            }

            return items;
        }

        [HttpPost]
        public int CreateCompany(string id)
        {
            try
            {
                var entity = new Company()
                {
                    Name = id,
                    Address = "ADD ADDRESS",
                    CityId = DataContext.Cities.FirstOrDefault().Id,
                    Phone = "00000000",
                    PostCode = "ADD POSTCODE",
                    County = "ADD COUNTY",

                };
                DataContext.Companies.Add(entity);
                DataContext.Commit();
                return entity.Id;
            }

            catch (Exception e)
            {
                return -1;
            }

        }


        [HttpPut]
        public bool? UpdatePriority(int? id)
        {
            try
            {

                Deal d = DataContext.Deals.FirstOrDefault(w => w.Id == id);
                d.Priority = !d.Priority;
                DataContext.Commit();
                return d.Priority;
            }

            catch (Exception e)
            {
                return null;
            }

        }

        [HttpPut]
        public bool? UpdateBroadcast(int? id)
        {
            try
            {

                Deal d = DataContext.Deals.FirstOrDefault(w => w.Id == id);
                if (d.Broadcast == null) d.Broadcast = false;
                d.Broadcast = !d.Broadcast;
                d.Priority = true;
                DataContext.Commit();
                return d.Broadcast;
            }

            catch (Exception e)
            {
                return null;
            }

        }

        [HttpPut]
        public bool? UpdateExpired(int? id)
        {
            try
            {

                Deal d = DataContext.Deals.FirstOrDefault(w => w.Id == id);
                d.Expired = d.Expired == null ? true : !d.Expired;
                DataContext.Commit();
                return d.Expired;
            }

            catch (Exception e)
            {
                return null;
            }

        }

        [HttpPut]
        [Route("expireAll/{id}/{expire}")]
        public HttpResponseMessage UpdateAllExpired(int? id, bool expire)
        {
            try
            {
                var season = DataContext.Seasons.FirstOrDefault(w => w.Id == id);

                if (season.Deals.Count == 0) return Request.CreateResponse(HttpStatusCode.NoContent, season.Name);

                foreach (var wonder in season.Deals)
                {
                    wonder.Expired = expire;
                }

                return Request.CreateResponse(HttpStatusCode.Created, expire);
            }

            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }

    } //end class


    public class SelectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }

}//end namespace

