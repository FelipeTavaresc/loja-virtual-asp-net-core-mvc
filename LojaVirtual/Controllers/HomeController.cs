using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LojaVirtual.Database;
using LojaVirtual.Libraries.Email;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using LojaVirtual.Repositories;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _repositoryCliente;
        private INewsletterRepository _repositoryNewsletter;
        private LoginCliente _loginCliente;
        public HomeController(IClienteRepository repositoryCliente, INewsletterRepository repositoryNewsletter,
            LoginCliente loginCliente)
        {
            _repositoryCliente = repositoryCliente;
            _repositoryNewsletter = repositoryNewsletter;
            _loginCliente = loginCliente;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewLetterEmail newLetter)
        {
            if (ModelState.IsValid)
            {
                _repositoryNewsletter.Cadastrar(newLetter);
                TempData["MSG_S"] = "E-mail cadastrado com sucesso";

                return RedirectToAction(nameof(Index));
            }else
            {
                return View();
            }
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            try
            {
                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var listaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid = Validator.TryValidateObject(contato, contexto, listaMensagens, true);

                if (isValid)
                {
                    ContatoEmail.EnviarContatoPorEmail(contato);

                    ViewData["MSG_S"] = "Mensagem enviada com sucesso";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var texto in listaMensagens)
                    {
                        sb.Append(texto.ErrorMessage + "<br />");
                    }

                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTATO"] = contato;
                }

                
            }
            catch (Exception e)
            {
                ViewData["MSG_E"] = "Ocorreu um erro, tente mais tarde!";

                //TODO - Implementar log
            }

            return View("Contato");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login([FromForm]Cliente cliente)
        {
            Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);
            if (clienteDB != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado";
                return View();
            }
        }


        [HttpGet]
        public IActionResult Painel()
        {
            Cliente cliente = _loginCliente.GetCliente();
            if (cliente != null)
            {
                return new ContentResult()
                { Content = "Usuário " + cliente.Id + 
                            ". E-mail: " + cliente.Email + 
                            " - Idade: " + DateTime.Now.AddYears(cliente.Nascimento.Year) + "Logado!" };
            }
            else
            {
                return new ContentResult() { Content = "Acesso negado." };
            }
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _repositoryCliente.Cadastrar(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso";

                return RedirectToAction(nameof(CadastroCliente));
            }

            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}