namespace WebApi.Services.SMTP
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;

    public class SendEmailService
    {
        public SendEmailService()
        {
        }
        public string sendMail(string to, string asunto, string body)
        {
            string msge = "Error al enviar este correo. Por favor verifique los datos o intente más tarde.";
            string from = "leosanchez_19@hotmail.com";
            string displayName = "Eduar Sanchez";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add(to);
                mail.Subject = asunto;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.office365.com", 587);
                client.Credentials = new NetworkCredential(from, "");
                client.EnableSsl = true;
                client.Send(mail);
                msge = "¡Correo enviado exitosamente! Pronto te contactaremos.";

            }
            catch (Exception ex)
            {
                msge = ex.Message + ". Por favor verifica tu conexión a internet y que tus datos sean correctos e intenta nuevamente.";
            }

            return msge;
        }
        public void sendMail(StringBuilder message, DateTime dateTime, string from, string to, string subject)
        {
            try
            {
                message.Append(Environment.NewLine);
                message.Append(string.Format("fecha {0:dd/MM/yyy}, hora {0:HH:mm:ss}", dateTime));
                message.Append(Environment.NewLine);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = message.ToString();
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("eduarleonardosanchez20@gmail.com", "1091658551");
                client.EnableSsl = true;
                client.Send(mail);
            }
            catch (Exception e)
            {

                throw ;
            }
        }
        public void sendMail(string from, string to, string subject, string host)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(to));
                mail.Subject = subject;
                string html = "<!DOCTYPE html><html><head><title>Correo de Ejemplo</title></head><body><h1>¡Hola, mundo!</h1><p>Este es un correo de ejemplo con formato HTML.</p><h2>Lista de elementos:</h2><ul><li>Elemento 1</li><li>Elemento 2</li><li>Elemento 3</li></ul><p>Puedes agregar más contenido HTML según tus necesidades, como enlaces, imágenes, tablas y estilos CSS.</p></body></html>\r\n";
                AlternateView alternate = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(alternate);
                smtp.Host = host;
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("eduarleonardosanchez20@gmail.com", "lhfgwfhorcgqklxd");
                smtp.EnableSsl = false;
                smtp.Send(mail);
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}
