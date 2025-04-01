using System;
using System.Net;
using System.Net.Mail;

namespace DeliverylogisticTracker
{
    class notification
    {
        private static string adminEmail = "bunnygreat0@gmail.com";
        private static string password = "jcwi dqnx vqvm wava";

        public static void sendNotification(string recipientEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(adminEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(adminEmail, password),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                Console.WriteLine($"Email sent to {recipientEmail}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {recipientEmail}.");
                Console.WriteLine(ex.Message);
            }
        }

        public static void sendnotiFication(string userEmail, string subject, string body)
        {
            sendNotification(adminEmail, subject, body);
            sendNotification(userEmail, subject, body);
        }
    }
}
