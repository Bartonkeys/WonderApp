using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WonderApp.Web.Controllers
{
    public class TagController : BaseApiController
    {

        private List<SelectItem> _tags = new List<SelectItem>();

        //TODO: hack for testing if DB read fails 
        private readonly List<SelectItem> _tagsLocal = new List<SelectItem>{
            new SelectItem {id = 1, text = "food"},
            new SelectItem {id = 2, text = "drink"},
            new SelectItem {id = 3, text = "shopping"},
            new SelectItem {id = 4, text = "entertainment"}  
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
    } //end class


    public class SelectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }

}//end namespace

