﻿using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Mail;
using System.Net;
using System.Net.Mail;
//using System.Web.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Utility
{
    public class EmailSendingUtility
    {
        public void Email_maaaahwanam(string txtto, string txtmessage, string subj)
        {
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("maaaahwanamtest@gmail.com");
            Msg.To.Add(txtto);
            //ExbDetails ex = new ExbDetails();
            Msg.Body = txtmessage;
            Msg.Subject = subj;
            Msg.IsBodyHtml = true;
            // your remote SMTP server IP.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ("SMTP.GMAIL.com").ToString();
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = ("maaaahwanamtest@gmail.com").ToString();
            NetworkCred.Password = ("maaaahwanamtest").ToString();
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(Msg);

            //Mail method for go daddy
            //string HostAdd = "relay-hosting.secureserver.net";
            //string FromEmailid = "maaaahwanamtest@gmail.com";
            //string Pass = "maaaahwanamtest";
            //string to = txtto.ToString();
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = FromEmailid;
            //mailMessage.Subject = subj;
            //mailMessage.BodyFormat = MailFormat.Html;
            //mailMessage.Body = txtmessage;
            //mailMessage.To = to;
            //System.Web.Mail.SmtpMail.SmtpServer = HostAdd;
            //System.Web.Mail.SmtpMail.Send(mailMessage);
            //mailMessage = null;
        }
    }
}