using BusinessLayer.Interfaces;
using DataLayer.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            try
            {
                Console.WriteLine($"emailTo: {emailTo}");
                Console.WriteLine($"subject: {subject}");
                Console.WriteLine($"message: {message}");

                // Validate emailTo and _mailSettings.Email
                if (string.IsNullOrWhiteSpace(emailTo))
                    throw new ArgumentException("Email recipient cannot be null or empty.", nameof(emailTo));


                if (string.IsNullOrWhiteSpace(_mailSettings.Email))
                    throw new ArgumentException("Sender email cannot be null or empty.", nameof(_mailSettings.Email));


                var em = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Email),
                    Subject = subject,
                };

                em.To.Add(MailboxAddress.Parse(emailTo));
                var builder = new BodyBuilder
                {
                    HtmlBody = message
                };
                em.Body = builder.ToMessageBody();
                em.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(em);
                smtp.Disconnect(true);
            }
            catch (ArgumentException ex)
            {
                // Catch and log specific argument exceptions
                Console.WriteLine($"Argument Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // General error logging
                Console.WriteLine($"Error in SendEmailAsync: {ex.Message}");
            }
        }

        public async Task SendEmailWithAttachmentsAsync(string emailTo, string subject, string message, List<IFormFile> files)
        {
            try
            {
                Console.WriteLine($"emailTo: {emailTo}");
                Console.WriteLine($"subject: {subject}");
                Console.WriteLine($"message: {message}");

                // Validate emailTo and _mailSettings.Email
                if (string.IsNullOrWhiteSpace(emailTo))
                    throw new ArgumentException("Email recipient cannot be null or empty.", nameof(emailTo));


                if (string.IsNullOrWhiteSpace(_mailSettings.Email))
                    throw new ArgumentException("Sender email cannot be null or empty.", nameof(_mailSettings.Email));

                var em = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Email),
                    Subject = subject,
                };

                em.To.Add(MailboxAddress.Parse(emailTo));
                var builder = new BodyBuilder
                {
                    HtmlBody = message
                };

                // Attach files if any
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            await file.CopyToAsync(ms); // Copy file contents to a memory stream
                            var fileBytes = ms.ToArray();

                            // Add the attachment to the email
                            builder.Attachments.Add(file.FileName, fileBytes);
                        }
                    }
                }

                em.Body = builder.ToMessageBody();
                em.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(em);
                smtp.Disconnect(true);
            }
            catch (ArgumentException ex)
            {
                // Catch and log specific argument exceptions
                Console.WriteLine($"Argument Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // General error logging
                Console.WriteLine($"Error in SendEmailWithAttachmentsAsync: {ex.Message}");
            }
        }

    }
}



