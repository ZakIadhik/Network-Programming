using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

class Program
{
    static async Task Main(string[] args)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("your_email@gmail.com"));
        email.To.Add(MailboxAddress.Parse("recipient@example.com"));
        email.Subject = "Тестовое письмо через MailKit";

        email.Body = new TextPart("plain")
        {
            Text = "Привет! Это тестовое письмо, отправленное через MailKit."
        };

        try
        {
            using var smtp = new SmtpClient();
            Console.WriteLine("Подключение к SMTP-серверу...");

            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            Console.WriteLine("Авторизация...");
            await smtp.AuthenticateAsync("ramin.nazarov.2004@gmail.com", "hvdykdqlorsahrrf");

            Console.WriteLine("Отправка письма...");
            await smtp.SendAsync(email);

            Console.WriteLine("Письмо успешно отправлено!");

            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.ResetColor();
        }
    }
}