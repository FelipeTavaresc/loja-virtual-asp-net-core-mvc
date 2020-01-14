using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            SmtpClient smtp = new SmtpClient("SMTP.office365.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("felipe.tavares@outlook.com", "");
            smtp.EnableSsl = true;

            string corpoMsg = string.Format("<h2>Contato - Loja Virtual</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                "<br /> E-mail enviado automaticamente do site Loja Virtual",
                contato.Nome,
                contato.Email,
                contato.Texto
           );

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress("felipe.tavares@outlook.com");
            mensagem.To.Add("felipe.tavares@outlook.com");
            mensagem.Subject = "Contato - Loja Virtual - E-mail:" + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            smtp.Send(mensagem);
        }
    }
}
