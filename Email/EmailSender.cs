// EmailSender.cs
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender
{
    private readonly string _host;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _login;
    private readonly string _password;

    public EmailSender(string host, int port, bool enableSsl, string login, string password)
    {
        _host = host;
        _port = port;
        _enableSsl = enableSsl;
        _login = login;
        _password = password;
    }

    public async Task SendAsync(EmailMessage message)
    {
        using var smtpClient = new SmtpClient(_host, _port)
        {
            EnableSsl = _enableSsl,
            Credentials = new NetworkCredential(_login, _password)
        };

        var mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body);

        try
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Подключение к SMTP-серверу...");
            Console.ResetColor();

            await smtpClient.SendMailAsync(mailMessage);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Письмо успешно отправлено!");
        }
        catch (SmtpException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка SMTP: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
        finally
        {
            Console.ResetColor();
        }
    }
}