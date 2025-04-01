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
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(adminEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(recipientEmail);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(adminEmail, password),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                Console.WriteLine($"Email sent successfully to {recipientEmail}.");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: Unable to send email to {recipientEmail}.");
                Console.WriteLine($"Error Details: {smtpEx.Message}");
            }
            catch (FormatException formatEx)
            {
                Console.WriteLine($"Format Error: The email address {recipientEmail} is not valid.");
                Console.WriteLine($"Error Details: {formatEx.Message}");
            }
            catch (ArgumentNullException argEx)
            {
                Console.WriteLine("Argument Null Error: One or more required arguments are null.");
                Console.WriteLine($"Error Details: {argEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Failed to send email to {recipientEmail}.");
                Console.WriteLine($"Error Details: {ex.Message}");
            }
        }

        public static void SendNotificationToAdminAndUser(string userEmail, string subject, string body)
        {
            try
            {
                sendNotification(adminEmail, subject, body);
                sendNotification(userEmail, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification to admin and user: {ex.Message}");
            }
        }
    }
}
