using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InżynierkaBiblioteka
{

    public static class WysylanieMaili
    {
        public static string Login;
        public static string Haslo;
        public static readonly string smtpGMail = "smtp.gmail.com";
        public static readonly int smtpGMailPortSSL = 467;
        public static readonly int smtpGMailPort = 587;

        public static void LogowanieDoMaila(string emailOdbiorcy, string smtpSerwer, int smtpPort)
        {
            using (var smtpClient = new SmtpClient(smtpSerwer,smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(Login, Haslo);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000; //10sek
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                using (MailMessage mm = new MailMessage("od@", emailOdbiorcy, "temat", "Wiadomosc"))
                {
                    try
                    {
                        smtpClient.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }



            }
        }





    }
}
