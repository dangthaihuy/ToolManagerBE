using Manager.WebApp.Models.System;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Net;
using System.Net.Mail;

namespace Manager.WebApp.Helpers.Business
{
    public class UserHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(UserHelpers));
        private static ICacheProvider _myCache;
        private static int _cacheExpiredTime = 10080;

        public static bool SendEmail(EmailModel model)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SystemSettings.EmailHost);

                mail.From = new MailAddress(model.Sender);

                if (!string.IsNullOrEmpty(model.Receiver))
                    mail.To.Add(model.Receiver);

               

                mail.Subject = model.Subject;
                mail.Body = model.Body;
                mail.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                mail.IsBodyHtml = true;
                SmtpServer.Port = Convert.ToInt32(SystemSettings.EmailServerPort);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(model.Sender, model.SenderPwd);
                SmtpServer.EnableSsl = SystemSettings.EmailIsUseSSL;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

                _logger.Error("Failed to send email: {0}", ex.ToString());
            }

            return true;
        }
    }
}
