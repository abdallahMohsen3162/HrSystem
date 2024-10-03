using BusinessLayer.Interfaces;
using DataLayer.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
//using SmtpClient = MailKit.Net.Smtp.SmtpClient;







namespace BusinessLayer.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly OutlookSettings _outlookSettings;
        public EmailSender(IOptions<MailSettings> options, IOptions<OutlookSettings> outlookOptions)
        {
            _mailSettings = options.Value;
            _outlookSettings = outlookOptions.Value;
            Console.WriteLine(_outlookSettings.EmailPassword);
        }

        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            //try
            //{
            //    Console.WriteLine($"emailTo: {emailTo}");
            //    Console.WriteLine($"subject: {subject}");
            //    Console.WriteLine($"message: {message}");

            //    // Validate emailTo and _mailSettings.Email
            //    if (string.IsNullOrWhiteSpace(emailTo))
            //        throw new ArgumentException("Email recipient cannot be null or empty.", nameof(emailTo));


            //    if (string.IsNullOrWhiteSpace(_mailSettings.Email))
            //        throw new ArgumentException("Sender email cannot be null or empty.", nameof(_mailSettings.Email));


            //    var em = new MimeMessage
            //    {
            //        Sender = MailboxAddress.Parse(_mailSettings.Email),
            //        Subject = subject,
            //    };

            //    em.To.Add(MailboxAddress.Parse(emailTo));
            //    var builder = new BodyBuilder
            //    {
            //        HtmlBody = message
            //    };
            //    em.Body = builder.ToMessageBody();
            //    em.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            //    using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            //    smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            //    await smtp.SendAsync(em);
            //    smtp.Disconnect(true);
            //}
            //catch (ArgumentException ex)
            //{
            //    // Catch and log specific argument exceptions
            //    Console.WriteLine($"Argument Error: {ex.Message}");
            //}
            //catch (Exception ex)
            //{
            //    // General error logging
            //    Console.WriteLine($"Error in SendEmailAsync: {ex.Message}");
            //}
        }

        public async Task SendEmailWithAttachmentsAsync(string emailTo, string subject, string message, List<IFormFile> files)
        {
            //try
            //{
            //    Console.WriteLine($"emailTo: {emailTo}");
            //    Console.WriteLine($"subject: {subject}");
            //    Console.WriteLine($"message: {message}");

            //    // Validate emailTo and _mailSettings.Email
            //    if (string.IsNullOrWhiteSpace(emailTo))
            //        throw new ArgumentException("Email recipient cannot be null or empty.", nameof(emailTo));


            //    if (string.IsNullOrWhiteSpace(_mailSettings.Email))
            //        throw new ArgumentException("Sender email cannot be null or empty.", nameof(_mailSettings.Email));

            //    var em = new MimeMessage
            //    {
            //        Sender = MailboxAddress.Parse(_mailSettings.Email),
            //        Subject = subject,
            //    };

            //    em.To.Add(MailboxAddress.Parse(emailTo));
            //    var builder = new BodyBuilder
            //    {
            //        HtmlBody = message
            //    };

            //    // Attach files if any
            //    if (files != null && files.Any())
            //    {
            //        foreach (var file in files)
            //        {
            //            if (file.Length > 0)
            //            {
            //                using var ms = new MemoryStream();
            //                await file.CopyToAsync(ms); 
            //                var fileBytes = ms.ToArray();

            //                builder.Attachments.Add(file.FileName, fileBytes);
            //            }
            //        }
            //    }
            //    em.Body = builder.ToMessageBody();
            //    em.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            //    using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            //    smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            //    await smtp.SendAsync(em);
            //    smtp.Disconnect(true);
            //}
            //catch (ArgumentException ex)
            //{
            //    Console.WriteLine($"Argument Error: {ex.Message}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error in SendEmailWithAttachmentsAsync: {ex.Message}");
            //}
        }





        public bool SendOutlookEmail(string email, string message)
        {
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"message: {message}");
            try
            {
                string sender = _outlookSettings.EmailSenderEmail;
                string password = _outlookSettings.EmailPassword;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_outlookSettings.SmtpHost);
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials 
                    = new System.Net.NetworkCredential(sender, password);
                client.EnableSsl = true; 
                client.Credentials = credentials;
                MailMessage emailMessage = new MailMessage
                (
                    new MailAddress(sender, _outlookSettings.EmailSenderName),
                    new MailAddress(email)
                    //Subject = _outlookSettings.ConfirmationEmailSubject,
                    //Body = message,
                    //IsBodyHtml = true
                );
                emailMessage.Subject = "subject";
                emailMessage.Body ="<h1>body</h1>";
                emailMessage.IsBodyHtml = true;

   
                client.Send(emailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; 
            }
        }







    }
}



