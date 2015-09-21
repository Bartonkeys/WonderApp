using WonderApp.Web.InfaStructure;

namespace WonderApp.Web.Extensions
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    #endregion

    public static class HtmlHelperExtensions
    {
        #region Public Static Methods

        public static MvcHtmlString ClientMessages(
            this HtmlHelper htmlHelper)
        {
            var messages = htmlHelper.ViewBag.ClientMessages as List<ClientMessage>;
            if (messages != null && messages.Count > 0)
            {
                string result = messages.Aggregate<ClientMessage, string>(null, (current, message) => current + GetMessageDivTag(message));

                return MvcHtmlString.Create(HttpUtility.HtmlDecode(result));
            }

            return MvcHtmlString.Empty;
        }

        #endregion

        #region Private Static Methods

        private static TagBuilder GetMessageDivTag(ClientMessage message)
        {

            var divTag = new TagBuilder("div");
            divTag.AddCssClass(string.Format("alert alert-{0} alert-dismissable alert-dynamic", message.Type));
            divTag.Attributes.Add("role", "alert");

            divTag.InnerHtml =
                "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>";

            divTag.InnerHtml += message.Text;

            return divTag;
        }

        #endregion
    }
}