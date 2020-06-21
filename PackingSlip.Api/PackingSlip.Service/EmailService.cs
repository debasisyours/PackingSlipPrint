using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Service
{
    public interface IEmailService
    {
        Task<bool> SendMail(string fromAddress, string toAddress, string subject, string body, 
                            string userCredential, string password,
                            string host, string port);
    }

    public class EmailService: IEmailService
    {
        public async Task<bool> SendMail(string fromAddress, string toAddress, string subject, string body, 
                                         string userCredential, string password, 
                                         string host, string port)
        {
            bool success = false;
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = Convert.ToInt32(port);
            client.Credentials = new NetworkCredential(userCredential, password);
            client.DeliveryFormat = SmtpDeliveryFormat.International;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(toAddress));
            mailMessage.From = new MailAddress(fromAddress);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            await client.SendMailAsync(mailMessage);
            success = true;
            return success;
        }
    }
}
