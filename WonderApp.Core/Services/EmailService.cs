using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using WonderApp.Contracts.DataContext;
using WonderApp.Data;

namespace WonderApp.Core.Services
{
  
    public class EmailService
    {
        //[Inject]
        //public static IDataContext DataContext { get; set; }

        public static List<AspNetUser> SendMyWonderEmails(IDataContext dataContext)
        {

            var usersToSendEmailTo = new List<AspNetUser>(dataContext.AspNetUsers.Where(u => u.UserPreference.EmailMyWonders));

            foreach (var user in usersToSendEmailTo)
            {
                CreateMyWondersEmailAndSend(user);   
            }

            return usersToSendEmailTo;

        }

        private static void CreateMyWondersEmailAndSend(AspNetUser user)
        {
            string emailtext = "MyWonders = \n";

            var wonders = user.MyWonders;
           //.Where(w => w.Archived == false
           //    && w.Expired != true
           //    && (w.AlwaysAvailable == true || w.ExpiryDate >= DateTime.Now)).ToList();

            foreach (var wonder in wonders)
            {
                emailtext += wonder.Title + "\n";
            }

            NotificationEmail email = new NotificationEmail();
            email.Created = DateTime.UtcNow;
            email.RecipientEmail = user.Email;
            email.RecipientName = user.UserName;

            SendEmailWithTemplate(email, emailtext);

        }

        private static void SendEmailWithTemplate(NotificationEmail email, string emailText)
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("emails@thewonderapp.co");

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
            {
                @"" + email.RecipientName + " <" + email.RecipientEmail + ">"
            };

            myMessage.AddTo(recipients);

            myMessage.Subject = "Here are your MyWonders";

            //Add the HTML and Text bodies
            myMessage.Html = "<p> "+ emailText +" </p>";
            myMessage.Text = emailText;

            // Create network credentials to access your SendGrid account.
            var username = "yerma";
            var pswd = "Y)rm91234";

            var credentials = new NetworkCredential(username, pswd);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            transportWeb.Deliver(myMessage);

        }
    }

    
}
