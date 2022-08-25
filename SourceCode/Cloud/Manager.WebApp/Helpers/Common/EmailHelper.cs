//using Amazon;
//using Amazon.Runtime;
//using Amazon.SimpleEmail;
//using Amazon.SimpleEmail.Model;
//using Manager.WebApp.Logging;
//using Manager.WebApp.Models;
//using Manager.WebApp.Settings;
//using Manager.SharedLibs.Extensions;
//using MimeKit;
//using System;
//using System.IO;
//using System.Net;
//using System.Net.Mail;
//using System.Security.Cryptography.X509Certificates;

//namespace Manager.WebApp.Helpers
//{
//    public class EmailHelper
//    {
//        private static readonly ILog logger = LogProvider.For<EmailHelper>();

//        public static string GetEmailTemplate(string templateName)
//        {
//            var content = string.Empty;           
//            try
//            {
//                //Read template file from the App_Data folder
//                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("~/Views/EmailTemplates/{0}.html", templateName));
//                using (var sr = new StreamReader(mappedPath))
//                {
//                    content = sr.ReadToEnd();
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.Error(string.Format("Failed when GettingEmailTemplate [{0}] due to {1}", templateName, ex.ToString()));
//            }

//            return content;
//        }

//        public static string SendEmailByAmazon(EmailModel model)
//        {
//            var result = string.Empty;

//            var accessKeyId = AWSSettings.AWS_SES_AccessKeyId;
//            var secrectAccessKey = AWSSettings.AWS_SES_SecrectAccessKey;

//            try
//            {
//                var credentals = new BasicAWSCredentials(accessKeyId, secrectAccessKey);

//                using (var client = new AmazonSimpleEmailServiceClient(credentals, RegionEndpoint.APSoutheast2))
//                {
//                    var message = new MimeMessage();
//                    message.From.Add(new MailboxAddress(AWSSettings.AWS_SES_EmailDisplayName, AWSSettings.AWS_SES_Email));
//                    message.To.Add(new MailboxAddress(string.Empty, model.Receiver));

//                    if (model.Receivers.HasData())
//                    {
//                        foreach (var item in model.Receivers)
//                        {
//                            message.To.Add(new MailboxAddress(string.Empty, item));
//                        }
//                    }
//                    message.Subject = model.Subject;

//                    var body = new BodyBuilder()
//                    {
//                        HtmlBody = model.Body
//                    };

//                    message.Body = body.ToMessageBody();

//                    var stream = new MemoryStream();
//                    message.WriteTo(stream);

//                    var sendRequest = new SendRawEmailRequest
//                    {
//                        RawMessage = new RawMessage(stream)
//                    };

//                    var response = client.SendRawEmail(sendRequest);
//                }
//            }
//            catch (Exception ex)
//            {
//                result = string.Format("Failed when sending email by Amazon due to {0}", ex.ToString());
//            }

//            return result;
//        }

//        public static string SendEmailBasic(EmailModel model)
//        {
//            var result = string.Empty;            
//            try
//            {
//                ServicePointManager.CertificatePolicy = new MyPolicy();
//                MailMessage mail = new MailMessage();
//                SmtpClient SmtpServer = new SmtpClient(SystemSettings.EmailHost);

//                mail.From = new MailAddress(model.Sender);

//                if(!string.IsNullOrEmpty(model.Receiver))
//                    mail.To.Add(model.Receiver);

//                if (model.Receivers.HasData())
//                {
//                    foreach (var item in model.Receivers)
//                    {
//                        mail.To.Add(item);
//                    }
//                }

//                mail.Subject = model.Subject;
//                mail.Body = model.Body;
//                mail.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
//                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

//                //Attachment attachment;
//                //attachment = new Attachment(@"E:\NHS\N_Document\N_SVN\VID\document\send_email_example.txt");
//                //mail.Attachments.Add(attachment);
//                mail.IsBodyHtml = true;
//                SmtpServer.Port = Convert.ToInt32(SystemSettings.EmailServerPort);
//                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
//                SmtpServer.UseDefaultCredentials = false;
//                SmtpServer.Credentials = new System.Net.NetworkCredential(model.Sender, model.SenderPwd);
//                SmtpServer.EnableSsl = SystemSettings.EmailIsUseSSL;

//                SmtpServer.Send(mail);
//            }
//            catch (Exception ex)
//            {
//                result = string.Format("Failed when sending email due to {0}", ex.ToString());
//            }

//            return result;
//        }

//        public static string SendEmail(EmailModel model)
//        {
//            var result = string.Empty;

//            if (AWSSettings.AWS_SES_Enabled)
//            {
//                result = SendEmailByAmazon(model);
//            }
//            else
//            {
//                result = SendEmailBasic(model);
//            }

//            return result;
//        }       


//        public static string SendEmailSample()
//        {
//            var result = string.Empty;
//            try
//            {
//                System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

//                string fromEmailAddress = SystemSettings.EmailSender;
//                string toEmailAddress = "bangvl@gmail.vn";
//                string email_Vefiry = SystemSettings.EmailSender;
//                string password_Vefiry = SystemSettings.EmailSenderPwd;
//                const string subject = "Test send from source code Subject";
//                const string body = @"<html>
//            <body>
//            <b color='red'> đây là nội dung xin chào {0} email gửi để test </b>
//             <body>
//             </html> ";
//                string bodyHtmlFormat = string.Format(body, toEmailAddress);
//                MailMessage mail = new MailMessage();
//                SmtpClient SmtpServer = new SmtpClient("mail.ancnc.vn");

//                mail.From = new MailAddress(fromEmailAddress);
//                mail.To.Add(toEmailAddress);
//                mail.Subject = subject;
//                mail.Body = bodyHtmlFormat;
//                //Attachment attachment;
//                //attachment = new Attachment(@"E:\NHS\N_Document\N_SVN\VID\document\send_email_example.txt");
//                //mail.Attachments.Add(attachment);
//                mail.IsBodyHtml = true;
//                SmtpServer.Port = 587;
//                SmtpServer.Credentials = new System.Net.NetworkCredential(email_Vefiry, password_Vefiry);
//                SmtpServer.EnableSsl = true;

//                SmtpServer.Send(mail);
//            }
//            catch (Exception ex)
//            {
//                result = string.Format("Failed when sending email due to {0}", ex.ToString());
//            }

//            return result;
//        }
//    }

//    public class EmailTemplateConst
//    {
//        public static string Sender = "{SENDER}";
//        public static string Receiver = "{RECEIVER}";
//        public static string RecoverPwdLink = "{RECOVER_PWD_LINK}";
//        public static string ActiveAccountLink = "{ACTIVE_ACCOUNT_LINK}";
//    }

//    public class MyPolicy : ICertificatePolicy
//    {
//        public MyPolicy()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
//        {
//            return true;
//        }
//    }
//}