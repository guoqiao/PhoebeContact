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

        public Email()
        {
            m_client.DeliveryMethod = SmtpDeliveryMethod.Network;

            XmlCare xc = new XmlCare("config.xml");
            XmlElement xe = xc.GetElement("mail");
            string smtp = xe.GetAttribute("smtp");
            int port = int.Parse(xe.GetAttribute("port"));
            bool ssl = xe.GetAttribute("ssl") == "true"; 
            string account = xe.GetAttribute("account");
            string password = xe.GetAttribute("password");

            m_client.Host = smtp;
            m_client.Port = port;
            m_client.EnableSsl = ssl;
            m_client.Credentials = new NetworkCredential(account, password);
            m_account = account;
        }

        public void SendMail(string receiver, string subject, string body)
        {
            MailMessage mail = new MailMessage(m_account, receiver, subject, body);
            mail.SubjectEncoding = Encoding.GetEncoding("GB2312");
            mail.BodyEncoding = Encoding.GetEncoding("GB2312");
            m_client.Send(mail);
        }
    }
}
