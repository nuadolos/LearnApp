using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHTTP.EmailService
{
    /// <summary>
    /// Отправляет шаблонное сообщение указанному пользователю
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig) => _emailConfig = emailConfig;

        /// <summary>
        /// Создает и отправляет сообщение получателю
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        /// <summary>
        /// Асинхронный метод SendEmail
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            // Создает письмо для получателя
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            // Создает конструктор для тела сообщения
            var bodyBuilder = new BodyBuilder { HtmlBody = String.Format("<h3>{0}</h3>", message.Content) };

            // Вставляет в тело письма добавленные отправителем файлы
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;

                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    // Добавляет в тело файл в двоичном виде
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            // Вставляет все необходимо в тело сообщения
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        /// <summary>
        /// Отправляет письмо указанному пользователю
        /// </summary>
        /// <param name="mailMessage"></param>
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    // Подключается к серверу
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Производит вход в учетную запись почты отправителя
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    // Отправляет сообщение
                    client.Send(mailMessage);
                }
                finally
                {
                    //Отключается от сервера
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод Send
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SendAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(emailMessage);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
