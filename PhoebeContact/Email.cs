using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Xml;

namespace PhoebeContact
{
    class Email
    {
        private SmtpClient m_client = new SmtpClient();
        string m_account = "";
        string m_displayname = "";

        public Email()
        {
            m_client.DeliveryMethod = SmtpDeliveryMethod.Network;

            XmlCare xc = new XmlCare("config.xml");
            XmlElement xe = xc.GetElement("mail");
            string smtp = xe.GetAttribute("smtp");
            int port = int.Parse(xe.GetAttribute("port"));
            bool ssl = xe.GetAttribute("ssl") == "true";
            string account = xe.GetAttribute("account");
            string displayname = xe.GetAttribute("displayname");
            string password = xe.GetAttribute("password");

            m_client.Host = smtp;
            m_client.Port = port;
            m_client.EnableSsl = ssl;
            m_client.Credentials = new NetworkCredential(account, password);
            m_account = account;
            m_displayname = displayname;
        }

        public void SendMail(string receiver, string subject, string body)
        {
            MailAddress from = new MailAddress(m_account, m_displayname);
            MailAddress to = new MailAddress(receiver);
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = body;
            mail.SubjectEncoding = Encoding.GetEncoding("GB2312");
            mail.BodyEncoding = Encoding.GetEncoding("GB2312");
            m_client.Send(mail);
        }
    }
}
