using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendVerificationEmail(string toEmail, string otpCode)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("giangdtph40542@gmail.com", "tfxpokeasdsmwijx"),
            EnableSsl = true,
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress("giangdtph40542@gmail.com"),
            Subject = "Xác thực tài khoản của bạn",
            Body = $"Chào bạn, mã OTP xác thực tài khoản của bạn là: {otpCode}",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);
        smtpClient.Send(mailMessage);
    }
}