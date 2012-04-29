using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace PhoebeContact
{
    class Email
    {
        private SmtpClient m_client = new SmtpClient("smtp.gmail.com", 587);
        string m_username = "name@gmail.com";
        string m_password = "pass";

        public Email()
        {
            m_client.DeliveryMethod = SmtpDeliveryMethod.Network;
            m_client.EnableSsl = true;
            m_client.Credentials = new NetworkCredential(m_username, m_password);
        }

        public void SendMail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(m_username, to, subject, body);
            mail.IsBodyHtml = true;
            mail.SubjectEncoding = Encoding.GetEncoding("GB2312");
            mail.BodyEncoding = Encoding.GetEncoding("GB2312");
            m_client.Send(mail);
        }
    }
}
