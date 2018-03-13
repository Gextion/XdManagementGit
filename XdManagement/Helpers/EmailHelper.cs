using System.Configuration;
using System.Net.Mail;

namespace EficienciaEnergetica.Helpers
{
    public static class EmailHelper
    {
        /// <summary>
        /// Send Passwrod Recovery Email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Pwd"></param>
        public static bool SendPwdRecoveryEmail(string Email, string Pwd)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpUsrDisplayName"]);
                    Mail.To.Add(new MailAddress(Email));

                    Mail.Subject = "XdManagement - Olvido de Contraseña";
                    Mail.IsBodyHtml = true;
                    Mail.Body = Properties.EmailTemplates.ForgotPwd.Replace("{PWD_NEW}", Pwd);

                    if (bool.Parse(ConfigurationManager.AppSettings["SmtpUseSecurityData"]))
                    {
                        smtp.UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["SmtpUseDefaultCredentials"]);
                        smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUsrCredentials"], ConfigurationManager.AppSettings["SmtpPwdCredentials"]);
                        smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                    }

                    smtp.Send(Mail);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}