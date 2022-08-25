using System.ComponentModel.DataAnnotations;
using Manager.WebApp.Settings;

namespace Manager.Business
{
    public class MailSettings: SettingsBase
    {        
        [Required]
        [Display(Name = "Server", Description = "Mail Server to send message through. Should be a domain name (mail.yourserver.net) or IP Address (211.123.123.123)")]
        public string SmtpServer { get; set; }


        [Required]
        [Display(Name = "Port", Description = "Port on the mail server to send through. Defaults to port 25.")]
        public int SmtpPort { get; set; }

        [Required]
        [Display(Name = "UseSSL", Description = "Use SSL communication when sending an email. Hack: client.EnableSsl = client.Port == 587 || client.Port == 465; client.EnableSsl = client.Port != 25;")]
        public bool SmtpUseSsl { get; set; }


        //[Required]
        [Display(Name = "Username", Description = "Username to login to the mail server.")]        
        public string SmtpUsername { get; set; }


        //[Required]
        [Display(Name = "Password", Description = "Password to login to the mail server.")]             
        public string SmtpPassword { get; set; }


        [Required]
        [Display(Name = "Timeout In Seconds", Description = "Connection timeouts for the mail server in seconds. If this timeout is exceeded waiting for a connection or for receiving or sending data the request is aborted and fails.")]           
        public int SmtpTimeout { get; set;}

        [Display(Name = "Email thank Cc list", Description = "The list of Cc emails will be included when the agency registered")]
        public string EmailThankCcList { get; set; }

        [Display(Name = "Email active Cc list", Description = "The list of Cc emails will be included when the active email was sent")]
        public string EmailActiveCcList { get; set; }

        [Display(Name = "Email active Admin Cc list", Description = "The list of Admin Cc emails will be included when the active email was sent")]
        public string EmailActiveAdminCcList { get; set; }
    }
}
