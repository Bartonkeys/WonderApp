using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RazorEngine;
using SendGrid;
using WonderApp.Contracts.DataContext;
using WonderApp.Data;
using Template = WonderApp.Data.Template;

namespace WonderApp.Core.Services
{
  
    public class EmailService
    {
        private static Template _templateToUse;
        private static IDataContext _dataContext;
       
        public static List<AspNetUser> SendMyWonderEmails(IDataContext dataContext)
        {
            _dataContext = dataContext;
            var usersToSendEmailTo = new List<AspNetUser>(_dataContext.AspNetUsers.Where(u => u.UserPreference.EmailMyWonders));

            foreach (var user in usersToSendEmailTo)
            {
                var email = CreateMyWondersEmailAndSend(user);
                if (email != null)
                {
                    _dataContext.NotificationEmails.Add(email);
                }
            }

            return usersToSendEmailTo;

        }

        private static NotificationEmail CreateMyWondersEmailAndSend(AspNetUser user)
        {
            string emailPlainText = "MyWonders = \n";
            string emailHtmlText = "";

            var email = new NotificationEmail
            {
                Created = DateTime.UtcNow,
                RecipientEmail = user.Email,
                RecipientName = user.UserName
            };

            var wonders = user.MyWonders;
           //.Where(w => w.Archived == false
           //    && w.Expired != true
           //    && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)).ToList();

            foreach (var wonder in wonders)
            {
                emailPlainText += wonder.Title + "\n";
            }


            Template templateToUse = _dataContext.Templates.FirstOrDefault(t => t.Name.Equals("MyWondersEmail"));
            if (templateToUse != null)
            {
                emailHtmlText = LoadTemplate(templateToUse.File.Trim(), user.MyWonders);
                email.Template_Id = templateToUse.Id;
            }
            else
            {
                emailHtmlText = emailPlainText;
            }
            
            return SendEmail(email, emailPlainText, emailHtmlText);

        }

        private static NotificationEmail SendEmail(NotificationEmail email, string emailPlainText, string emailHtmlText)
        {
            try
            {
                // Create the email object first, then add the properties.
                var emailMessage = new SendGridMessage();

                // Add the message properties.
                emailMessage.From = new MailAddress("emails@thewonderapp.co");

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                    @"" + email.RecipientName + " <" + email.RecipientEmail + ">"
                };

                emailMessage.AddTo(recipients);

                emailMessage.Subject = "Here are your MyWonders";

               
                //Add the HTML and Text bodies
                emailMessage.Html = emailHtmlText;
                emailMessage.Text = emailPlainText;

                // true indicates that links in plain text portions of the email 
                // should also be overwritten for link tracking purposes. 
                emailMessage.EnableClickTracking(true);

                // Create network credentials to access your SendGrid account.
                var username = "yerma";
                var pswd = "Y)rm91234";

                var credentials = new NetworkCredential(username, pswd);

                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email.
                transportWeb.Deliver(emailMessage);

                email.Sent = DateTime.UtcNow;

                return email;
            }

            catch (Exception e)
            {
                return email;
            }

        }



        public static string LoadTemplate(string template, object viewModel)
        {
            var templateContent = AttemptLoadEmailTemplate(template);
            var compiledTemplate = Razor.Parse(templateContent, viewModel);

            return compiledTemplate;
        }

        private static string AttemptLoadEmailTemplate(string name)
        {
            if (File.Exists(name))
            {
                var templateText = File.ReadAllText(name);
                return templateText;
            }

            var templateName = string.Format("~/EmailTemplates/{0}.cshtml", name); //Just put your path to a scpecific template
            var emailTemplate = HttpContext.Current.Server.MapPath(templateName);

            if (File.Exists(emailTemplate))
            {
                var templateText = File.ReadAllText(emailTemplate);
                return templateText;
            }

            return null;
        }

#region "Currently unused methods for Email Attachments"
        //public bool SendEmailMessage(string template, object viewModel, string to, string @from, string subject, params string[] replyToAddresses)
        //{
        //    var compiledTemplate = LoadTemplate(template, viewModel);

        //    return SendEmail(from, to, subject, compiledTemplate, from, null, replyToAddresses);

        //}

        //public bool SendEmailMessageWithAttachments(string template, object viewModel, string to, string @from, string subject, List<Attachment> attachedFiles, params string[] replyToAddresses)
        //{
        //    var compiledTemplate = LoadTemplate(template, viewModel);
        //    return SendEmail(from, to, subject, compiledTemplate, from, attachedFiles, replyToAddresses);
        //}

        //private bool SendEmail(string from, string to, string subject, string body, string replyTo, List<Attachment> attachedFiles, params string[] replyToAddresses)
        //{
        //    replyTo = replyTo ?? from;
        //    attachedFiles = attachedFiles ?? new List<Attachment>();

        //    var message = new MailMessage(from, to, subject, body);
        //    message.ReplyToList.Add(replyTo);

        //    foreach (var attachedFile in attachedFiles)
        //        message.Attachments.Add(attachedFile);

        //    try
        //    {
        //        smtpClient.SendAsync(email, null);
        //        return true;
        //    }
        //    catch (Exception exption)
        //    {
        //        return false;
        //    }
        //}
    
#endregion

    }





    
}
