using Manager.SharedLibs;
using Manager.WebApp.Models.System;
using Manager.WebApp.Settings;
using Serilog;
using System;
using System.Net;
using System.Net.Mail;

namespace Manager.WebApp.Helpers.Business
{
    public class EmailHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(EmailHelpers));
        private static ICacheProvider _myCache;
        private static int _cacheExpiredTime = 10080;

        public static bool SendEmail(EmailModel model)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(AppConfiguration.GetAppsetting("SystemSetting:SmtpGmail:Host"));

                mail.From = new MailAddress(model.Sender);

                if (!string.IsNullOrEmpty(model.Receiver))
                    mail.To.Add(model.Receiver);

               

                mail.Subject = model.Subject;
                mail.Body = model.Body;
                mail.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                mail.IsBodyHtml = true;
                SmtpServer.Port = Convert.ToInt32(AppConfiguration.GetAppsetting("SystemSetting:SmtpGmail:Port"));
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(model.Sender, model.SenderPwd);
                SmtpServer.EnableSsl = true;

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
