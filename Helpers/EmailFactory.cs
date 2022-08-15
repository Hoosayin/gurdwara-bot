using GurdwaraBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GurdwaraBot.Helpers
{
    public class EmailFactory
    {
        public static void SendFeedbackMail(FeedbackData feedbackData)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    Credentials = new NetworkCredential("gurdwara.sri.panja.sahib@gmail.com", "LP2twigs"),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage("gurdwara.sri.panja.sahib@gmail.com", "gurdwara.sri.panja.sahib@gmail.com")
                {
                    Subject = $"Sri Panja Sahib - Confirmation Code",
                    Body = $"<table><tr><td style = \"padding: 20px; border: solid 5px #DADADA; color: #505050;; background-color: #F4F4F4; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif\"><img src = \"https://pp.userapi.com/c847018/v847018625/1d7ecd/7xqdG3br0TA.jpg\" height = \"50\" style = \"border-radius: 50%;\" /><h3>Gurdwara Sri Panja Sahib</h3><hr /><p>Feedback Type: {feedbackData.FeedbackType}\r\nComment: {feedbackData.Comment}\r\nRating: {feedbackData.Rating}</p></td></tr></table>",
                };

                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SendConfirmationCode(string to)
        {
            string confirmationCode = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();

            try
            {
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    Credentials = new NetworkCredential("gurdwara.sri.panja.sahib@gmail.com", "LP2twigs"),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage("gurdwara.sri.panja.sahib@gmail.com", to)
                {
                    Subject = $"Sri Panja Sahib - Confirmation Code",
                    Body = $"<table><tr><td style = \"padding: 20px; border: solid 5px #DADADA; color: #505050;; background-color: #F4F4F4; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif\"><img src = \"https://pp.userapi.com/c847018/v847018625/1d7ecd/7xqdG3br0TA.jpg\" height = \"50\" style = \"border-radius: 50%;\" /><h3>Gurdwara Sri Panja Sahib</h3><hr /><p>Your confirmation code is {confirmationCode}</p></td></tr></table>",
                };

                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                return confirmationCode;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
