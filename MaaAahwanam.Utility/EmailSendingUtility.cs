using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Mail;
using System.Net;
using System.Net.Mail;
//using System.Web.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Net.Mail;

namespace MaaAahwanam.Utility
{
    public class EmailSendingUtility
    {
        public class EmailModel
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        public void Email_maaaahwanam(string txtto, string txtmessage, string subj, HttpPostedFileBase fileUploader)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("info@knotsandvows.co.in");
                //var mb = "dedeepya@gmail.com,sneha.akula9@gmail.com,prabodh.dasari@xsilica.com,info@knotsandvows.co.in"; //prod
                var mb = "sneha.akula9@gmail.com,prabodh.dasari@xsilica.com"; //sandbox
                Msg.Bcc.Add(mb);
                Msg.To.Add(txtto);
                //ExbDetails ex = new ExbDetails();
                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    Msg.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                }
                Msg.Body = txtmessage;
                Msg.Subject = subj;
                Msg.IsBodyHtml = true;
                //your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ("relay-hosting.secureserver.net").ToString();
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ("info@knotsandvows.co.in").ToString();
                NetworkCred.Password = ("master@1234").ToString();
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(Msg);

            }
            catch (Exception ex)
            {
                throw ex;
                //message = "failed";
            }


            //Mail method for go daddy
            //string HostAdd = "relay-hosting.secureserver.net";
            //string FromEmailid = "info@knotsandvows.co.in";
            //string Pass = "spreadinghappiness";
            //string to = txtto.ToString();
            //string displayname = "SevenVows";
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.Bcc = "dedeepya@gmail.com,sneha.akula9@gmail.com,prabodh.dasari@xsilica.com";
            //mailMessage.From = FromEmailid;
            //mailMessage.Subject = subj;
            //mailMessage.BodyFormat = MailFormat.Html;
            //mailMessage.Body = txtmessage;
            //mailMessage.To = to;
            //mailMessage.Priority = MailPriority.High;
            //System.Web.Mail.SmtpMail.SmtpServer = HostAdd;
            //System.Web.Mail.SmtpMail.Send(mailMessage);
            //mailMessage = null;


        }

      
        public void Email_maaaahwanam1(string txtto, string txtmessage, string subj, string from)
        {
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("sevenvows@sevenvows.co.in");

            Msg.To.Add(txtto);
            //ExbDetails ex = new ExbDetails();
            //if (fileUploader != null)
            //{
            //    string fileName = Path.GetFileName(fileUploader.FileName);
            //    Msg.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
            //}
            Msg.Body = txtmessage;
            Msg.Subject = subj;
            Msg.IsBodyHtml = true;
            //your remote SMTP server IP.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ("relay-hosting.secureserver.net").ToString();
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = ("weddingplanner@knotsandvows.co.in").ToString();
            NetworkCred.Password = ("master@1234").ToString();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.Send(Msg);

            //Mail method for go daddy
            //string HostAdd = "relay-hosting.secureserver.net";    
            //string FromEmailid = "weddingplanner@knotsandvows.co.in";
            //string Pass = "spreadinghappiness";
            //string displayname = "SevenVows";
            //string to = txtto.ToString();
            //MailMessage mailMessage = new MailMessage();
            ////mailMessage.Bcc = "dedeepya@gmail.com,sneha.akula9@gmail.com,prabodh.dasari@xsilica.com";
            //mailMessage.From = FromEmailid;
            //mailMessage.Subject = subj;
            //mailMessage.BodyFormat = MailFormat.Html;
            //mailMessage.Body = txtmessage;
            //mailMessage.To = to;
            //System.Web.Mail.SmtpMail.SmtpServer = HostAdd;
            //System.Web.Mail.SmtpMail.Send(mailMessage);
            //mailMessage = null;
        }



        public void testEmail_maaaahwanam(string txtto, string txtmessage, string subj, HttpPostedFileBase fileUploader)
        {
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("chpavan@maa-aahwanam.com");
            Msg.To.Add(txtto);
            //ExbDetails ex = new ExbDetails();
            //if (fileUploader != null)
            //{
            //    string fileName = Path.GetFileName(fileUploader.FileName);
            //    Msg.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
            //}
            Msg.Body = txtmessage;
            Msg.Subject = subj;
            Msg.IsBodyHtml = true;
            //your remote SMTP server IP.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ("relay-hosting.secureserver.net").ToString();
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = ("chpavan@maa-aahwanam.com").ToString();
            NetworkCred.Password = ("test@1234").ToString();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.Send(Msg);

            //Mail method for go daddy
            //string HostAdd = "relay-hosting.secureserver.net";    
            //string FromEmailid = "weddingplanner@knotsandvows.co.in";
            //string Pass = "spreadinghappiness";
            //string displayname = "SevenVows";
            //string to = txtto.ToString();
            //MailMessage mailMessage = new MailMessage();
            ////mailMessage.Bcc = "dedeepya@gmail.com,sneha.akula9@gmail.com,prabodh.dasari@xsilica.com";
            //mailMessage.From = FromEmailid;
            //mailMessage.Subject = subj;
            //mailMessage.BodyFormat = MailFormat.Html;
            //mailMessage.Body = txtmessage;
            //mailMessage.To = to;
            //System.Web.Mail.SmtpMail.SmtpServer = HostAdd;
            //System.Web.Mail.SmtpMail.Send(mailMessage);
            //mailMessage = null;
        }

        public void Wordpress_Email(string txtto, string txtmessage, string subj, HttpPostedFileBase fileUploader)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("weddingplanner@knotsandvows.co.in");
                //var mb1 = "weddingplanner@knotsandvows.co.in"; //prod
                // Msg.Bcc.Add(mb1);
                Msg.To.Add(txtto);
                //ExbDetails ex = new ExbDetails();
                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    Msg.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                }
                Msg.Body = txtmessage;

                Msg.IsBodyHtml = true;
                //your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ("relay-hosting.secureserver.net").ToString();
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ("weddingplanner@knotsandvows.co.in").ToString();
                NetworkCred.Password = ("master@1234").ToString();
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(Msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Mail method for go daddy
            //string HostAdd = "relay-hosting.secureserver.net";
            //string FromEmailid = "weddingplanner@knotsandvows.co.in";
            //string Pass = "spreadinghappiness";
            //string displayname = "SevenVows";
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