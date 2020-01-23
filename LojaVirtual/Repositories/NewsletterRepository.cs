using LojaVirtual.Database;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private LojaVirtualContext _banco;
        public NewsletterRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        public void Cadastrar(NewLetterEmail newsletter)
        {
            _banco.NewLetterEmails.Add(newsletter);
            _banco.SaveChanges();
        }

        public IEnumerable<NewLetterEmail> ObterTodasNewsletter()
        {
            return _banco.NewLetterEmails.ToList();
        }
    }
}