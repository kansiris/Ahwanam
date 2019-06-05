﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
//using System.Web.Mail;//host email use
using System.IO;
using System.Configuration;

namespace MaaAahwanam.Utility
{
    public class EmailUtility
    {
        public static void sendMailfromLocal(string MailToAddress, string SubjectDetails, string BodyContent)
        {
            string HostAdd = ConfigurationManager.AppSettings["Host"].ToString();//smtp host use;
            string FromEmailid = ConfigurationManager.AppSettings["FromMail"]; ;
            string Pass = ConfigurationManager.AppSettings["Password"].ToString();

            string to = MailToAddress + ";" + FromEmailid;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid);
            mailMessage.Subject = SubjectDetails;
            mailMessage.Body = BodyContent;
            mailMessage.IsBodyHtml = true;
            foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(address);
            }
            SmtpClient smtp = new SmtpClient();
            smtp.Host = HostAdd;
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = Pass;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage);
            mailMessage = null;
        }

        //public static void sendMailfromLocal(string MailToAddress, string SubjectDetails, string BodyContent)
        //{
        //    try
        //    {
        //        dynamic MailTo = null;
        //        dynamic MailFrom = null;
        //        dynamic NumberOfMails = null;
        //        dynamic i = null;
        //        i = 1;
        //        MailTo = MailToAddress + ";" + ConfigurationManager.AppSettings["FromMail"];
        //        MailFrom = ConfigurationManager.AppSettings["FromMail"].ToString();
        //        string[] add = MailTo.Split(';');
        //        NumberOfMails = add.Count();
        //        if (!string.IsNullOrEmpty(MailTo))
        //        {
        //            for (i = 1; i <= NumberOfMails; i++)
        //            {
        //                System.Web.Mail.MailMessage objMail = new System.Web.Mail.MailMessage();
        //                objMail.From = MailFrom;
        //                objMail.To = MailTo;
        //                objMail.Subject = " Packing List From ShreeImpex "; ;
        //                objMail.BodyFormat = MailFormat.Html;
        //                //MailFormat.Text to send plain text email
        //                objMail.Priority = MailPriority.High;
        //                //objMail.Body = "This test email was sent at: " + DateTime.Now;
        //                objMail.Subject = SubjectDetails;
        //                objMail.Body += BodyContent;
        //                System.Web.Mail.SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
        //                System.Web.Mail.SmtpMail.Send(objMail);
        //                objMail = null;
        //            }
        //        }
        //    }
        //    catch (Exception ew) { }
        //}



    }
}
