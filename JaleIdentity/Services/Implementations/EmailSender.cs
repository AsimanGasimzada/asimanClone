using JaleIdentity.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JaleIdentity.Services.Implementations;

public class EmailSender : IEmailSender
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        // Set up SMTP client
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("rampagehelpp@gmail.com", "nhzh qolm slip iifw");


        // Create email message
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("rampagehelpp@gmail.com");
        mailMessage.To.Add(toEmail);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;

        StringBuilder mailBody = new StringBuilder();

        mailBody.AppendFormat(body);

        mailMessage.Body = mailBody.ToString();





        // Send email
        client.Send(mailMessage);
    }
}
