using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WonderApp.Models;

namespace WonderApp.Core.Utilities
{
    public static class ExtensionMethods
    {
        public static IEnumerable<SelectListItem> ToCheckBoxListSource<T>(this IEnumerable<T> checkedCollection,
            IEnumerable<T> allCollection)
            where T : BaseModel
        {
            var result = new List<SelectListItem>();

            foreach (var allItem in allCollection)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = allItem.ToString();
                selectItem.Value = allItem.Id.ToString();
                selectItem.Selected = (checkedCollection.Count(c => c.Id == allItem.Id) > 0);

                result.Add(selectItem);
            }

            return result;
        }


        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name,
                                         IEnumerable<SelectListItem> items)
        {
            var output = new StringBuilder();
            output.Append(@"<div class=""checkboxList"">");

            foreach (var item in items)
            {
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");

                if (item.Selected)
                    output.Append(@" checked=""checked""");

                output.Append(" />");
                output.Append(item.Text);
                output.Append("<br />");
            }

            output.Append("</div>");

            return new MvcHtmlString(output.ToString());
        }

        public static void UpdateCollectionFromModel<T>(this ICollection<T> domainCollection,
                                                IQueryable<T> objects, int[] newValues)
                                                where T : BaseModel
        {
            if (newValues == null)
            {
                domainCollection.Clear();
                return;
            }

            for (var i = domainCollection.Count - 1; i >= 0; i--)
            {
                var domainObject = domainCollection.ElementAt(i);
                if (!newValues.Contains(domainObject.Id))
                    domainCollection.Remove(domainObject);
            }

            foreach (var newId in newValues)
            {
                var domainObject = domainCollection.FirstOrDefault(t => t.Id == newId);
                if (domainObject != null)
                    continue;

                domainObject = objects.FirstOrDefault(t => t.Id == newId);
                if (domainObject == null)
                {
                    continue;
                }

                domainCollection.Add(domainObject);
            }
        }

    }


}

