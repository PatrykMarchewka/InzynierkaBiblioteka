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
        private static SmtpClient? GetSmtpClient;

        public static void LogowanieDoMaila(string smtpSerwer = "smtp-mail.outlook.com", int smtpPort = 587, string Login = "InzynieriaBiblioteka@outlook.com", string Haslo = "myy4mgwo5k")
        {
                GetSmtpClient = new SmtpClient(smtpSerwer, smtpPort);
                GetSmtpClient.UseDefaultCredentials = false;
                GetSmtpClient.Credentials = new NetworkCredential(Login, Haslo);
                GetSmtpClient.EnableSsl = true;
                GetSmtpClient.Timeout = 10000; //10sek
                GetSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }


        public static void WysylanieWiadomosciEmail(string emailOdbiorcy, string temat, string wiadomosc)
        {
            if (GetSmtpClient == null)
            {
                try
                {
                    LogowanieDoMaila();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Blad! Nie mozna zalogowac sie do klienta SMTP: {ex.Message}");
                }
            }

            if (emailOdbiorcy != null)
            {
                try
                {
                    using (MailMessage mm = new MailMessage("InzynieriaBiblioteka@outlook.com", emailOdbiorcy, temat, wiadomosc))
                    {
                        GetSmtpClient.Send(mm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Blad! {ex.Message}");
                }
            }
                
        }




    }
}
