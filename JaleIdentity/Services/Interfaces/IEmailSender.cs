namespace JaleIdentity.Services.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail,string subject,string body);
    }
}
