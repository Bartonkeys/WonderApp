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
using AutoMapper;
using RazorEngine;
using SendGrid;
using WonderApp.Contracts.DataContext;
using WonderApp.Data;
using WonderApp.Models;
using Encoding = System.Text.Encoding;
using Template = WonderApp.Data.Template;

namespace WonderApp.Core.Services
{
  
    public class EmailService
    {
        private Template _templateToUse;
        private IDataContext _dataContext;
        private const int NumberOfWonders = 10;
        async public Task<List<AspNetUser>> SendMyWonderEmails(IDataContext dataContext)
        {
            _dataContext = dataContext;
            var usersToSendEmailTo = new List<AspNetUser>(_dataContext.AspNetUsers.Where(u => 
                u.UserPreference.EmailMyWonders &&
                u.UserPreference.Reminder!= null));
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            foreach (var user in usersToSendEmailTo)
            {
                var timeToCheck = user.UserPreference.Reminder.Time.ToLower().Equals("weekly")
                    ? oneWeekAgo
                    : oneMonthAgo;
                //Check time of last send  
                if (!dataContext.NotificationEmails.Any() ||
                    (dataContext.NotificationEmails.Any()
                    && !dataContext.NotificationEmails.Any(e => e.RecipientEmail == user.Email && e.Sent > timeToCheck)))
                {
                    var email = await CreateMyWondersEmailAndSend(user);
                    if (email != null)
                    {
                        _dataContext.NotificationEmails.Add(email);
                    }
                }
         
            }

            _dataContext.Commit();

            return usersToSendEmailTo;

        }

        private async Task <NotificationEmail> CreateMyWondersEmailAndSend(AspNetUser user)
        {
            string emailPlainText = "MyWonders = \n";
            string emailHtmlText = "";

            var email = new NotificationEmail
            {
                Created = DateTime.UtcNow,
                RecipientEmail = user.Email,
                RecipientName = user.UserName
            };

            var amountToSkip = user.MyWonders.Count <= NumberOfWonders ? 0 : user.MyWonders.Count - NumberOfWonders;
            var recentWonders = user.MyWonders.Where(x => x.Archived != true).Skip(amountToSkip).Reverse();
            //var recentWonders = user.MyWonders.Skip(user.MyWonders.Count - NumberOfWonders);
          
            var model = new EmailTemplateViewModel();
            model.User = Mapper.Map<UserModel>(user);
            model.Wonders = Mapper.Map<List<DealModel>>(recentWonders);

            //TODO: move these to config properties
            model.UrlString = "https://cms.thewonderapp.co/content/images/";
            model.UnsubscribeLink = "mailto:unsubscribe@thewonderapp.co";

            foreach (var wonder in recentWonders)
            {
                emailPlainText += wonder.Title + "\n";
            }


            Template templateToUse = _dataContext.Templates.FirstOrDefault(t => t.Name.Equals("MyWondersEmail"));
            if (templateToUse != null)
            {
                emailHtmlText = await LoadTemplate(templateToUse.File.Trim(), model);
                email.Template_Id = templateToUse.Id;
            }
            else
            {
                emailHtmlText = emailPlainText;
            }
            
            return await SendEmail(email, emailPlainText, emailHtmlText);

        }

        private async Task<NotificationEmail> SendEmail(NotificationEmail email, string emailPlainText, string emailHtmlText)
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
                await transportWeb.DeliverAsync(emailMessage);

                email.Sent = DateTime.UtcNow;

                return email;
            }

            catch (Exception e)
            {
                return email;
            }

        }



        public async Task<string> LoadTemplate(string template, object viewModel)
        {
            var templateContent = await AttemptLoadEmailTemplate(template);
            var compiledTemplate = Razor.Parse(templateContent, viewModel);

            return compiledTemplate;
        }

        private async Task<string> AttemptLoadEmailTemplate(string name)
        {
            if (File.Exists(name))
            {
                var templateText = await FileEx.ReadAllTextAsync(name);
                return templateText;
            }

            var templateName = string.Format("~/Views/EmailTemplates/{0}.cshtml", name); //Just put your path to a scpecific template
            var emailTemplate = HttpContext.Current.Server.MapPath(templateName);

            if (File.Exists(emailTemplate))
            {
                var templateText = await FileEx.ReadAllTextAsync(emailTemplate);
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



    public static class FileEx
    {
        public static Task<string[]> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }

        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(path, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }


        public static Task<string> ReadAllTextAsync(string path)
        {
            return ReadAllTextAsync(path, Encoding.UTF8);
        }
        public static async Task<string> ReadAllTextAsync(string path, Encoding encoding)
        {
            string alltext = "";

            using (var reader = new StreamReader(path, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!line.StartsWith("@model"))
                    {alltext += line;}
                }
            }

            return alltext;
        }
    }


    
}
