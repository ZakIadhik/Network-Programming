using FavoriteMoviesApp.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration; 
using System;

namespace FavoriteMoviesApp.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly PdfService _pdfService;

        public EmailService(IConfiguration config, PdfService pdfService)
        {
          
            _smtpServer = config["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SmtpServer is not configured in appsettings.json");
            _smtpUsername = config["EmailSettings:SmtpUsername"] ?? throw new InvalidOperationException("SmtpUsername is not configured in appsettings.json");
            _smtpPassword = config["EmailSettings:SmtpPassword"] ?? throw new InvalidOperationException("SmtpPassword is not configured in appsettings.json");

        
            var smtpPortString = config["EmailSettings:SmtpPort"];
            if (string.IsNullOrEmpty(smtpPortString) || !int.TryParse(smtpPortString, out _smtpPort))
            {
                throw new InvalidOperationException("SmtpPort is not configured or is invalid in appsettings.json");
            }

            _pdfService = pdfService;
        }

        public async Task SendFavoritesEmail(string email, string userName, List<MovieDto> movies)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Favorite Movies App", _smtpUsername));
            message.To.Add(new MailboxAddress(userName, email));
            message.Subject = "Your Favorite Movies";

            var builder = new BodyBuilder();
            builder.HtmlBody = GenerateHtmlEmail(userName, movies.Count);

         
            var chunks = movies.Chunk(5);
            int partNumber = 1;

            foreach (var chunk in chunks)
            {
                var pdfBytes = _pdfService.GeneratePdf(chunk, userName);
                builder.Attachments.Add($"favorites_part{partNumber}.pdf", pdfBytes, new ContentType("application", "pdf"));
                partNumber++;
            }

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private string GenerateHtmlEmail(string userName, int movieCount)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplate.html");
            var template = File.ReadAllText(templatePath);
            return template
                .Replace("{UserName}", userName)
                .Replace("{MovieCount}", movieCount.ToString());
        }
    }
}