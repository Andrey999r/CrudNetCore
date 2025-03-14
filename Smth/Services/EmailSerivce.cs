using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Smth.Data;
using Smth.Interfaces;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendSurveyInvitation(string recipientEmail, string surveyLink)
    {
        try
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,  // Включаем SSL
                Timeout = 20000 // Увеличиваем таймаут (20 секунд)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Приглашение пройти опрос",
                Body = $"Пройдите опрос по ссылке: {surveyLink}",
                IsBodyHtml = true
            };

            mail.To.Add(recipientEmail);
            client.Send(mail);

            Console.WriteLine($"Письмо успешно отправлено на {recipientEmail}");
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine("Ошибка SMTP: " + smtpEx.Message);
            throw new Exception("Ошибка SMTP: " + smtpEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка отправки письма: " + ex.Message);
            throw new Exception("Ошибка отправки письма: " + ex.Message);
        }
    }
}