using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Ninject;
using WonderApp.Contracts.DataContext;
using WonderApp.Data;

namespace WonderApp.Core.Services
{
    public class TagService
    {
        //[Inject]
        //public static IDataContext DataContext { get; set; }

        public static List<int> GetOrCreateTagIdsFromString(string tagString, IDataContext dataContext)
        {
            var tagIdList = new List<int>();

            var tagList = tagString.Split(',').ToList();
            foreach (var tag in tagList)
            {
                int tagId = -1;
                bool isString = char.IsLetter(tag.FirstOrDefault());
                if (isString)
                {
                    //create new Tag and set tagId
                    var newTag = new Tag();
                    newTag.Name = tag;
                    dataContext.Tags.Add(newTag);
                    dataContext.Commit();
                    tagId = newTag.Id;
                }
                else
                {
                    int parsedInt;
                    bool isValid = int.TryParse(tag, out parsedInt); // the out keyword allows the method to essentially "return" a second value
                    if (isValid)
                    {
                        tagId = parsedInt;
                    }
                }

                if (tagId != -1)
                {
                    tagIdList.Add(tagId);
                }
                
            }
             //= tagList.Select(x => new Tag { Name = x }).ToList();
    

            return tagIdList;

        }
    }
}
