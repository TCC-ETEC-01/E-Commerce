using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioController (FuncionarioRepositorio funcionario)
        {
            _funcionarioRepositorio = funcionario;
        }
        public IActionResult Index()
        {
            return View(_funcionarioRepositorio.TodosFuncionarios());
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var funcionario = _funcionarioRepositorio.ObterFuncionarioEmail(email);
            if (funcionario != null && funcionario.Senha == senha)
            {
                HttpContext.Session.SetString("FuncionarioLogado", funcionario.Email);
                ViewData["Mensagem"] = "Bem vindo" + funcionario.Email;
                RedirectToAction("Index", "Funcionario");
            }
            ViewBag.Erro = "Dados incorretos, tente novamente!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewData["Mesangem"] = "Você está indo embora? Até breve!";
            return RedirectToAction("Login","Funcionario");
        }

        public IActionResult CadastrarFuncionario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarFuncionario(tbFuncionario funcionario)
        {
            if (!int.TryParse(funcionario.Cpf, out _)|| !int.TryParse(funcionario.Telefone, out _))
            {
                ViewData["MensagemErro"] = "No campo CPF  e Telefone apenas numeros!";
            }
            return View();
        }

        public IActionResult EditarFuncionario(int Id)
        {
            var funcionario = _funcionarioRepositorio.ObterFuncionarioID(Id);
            if(funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }

        [HttpPost]
        public IActionResult EditarFuncionario(int Id, [Bind("IdFuncionario,Nome,Sexo,Email,Telefone,Cargo,Cpf,Senha")] tbFuncionario funcionario)
        {
            if(Id != funcionario.IdFuncionario)
            {
                return BadRequest();
            } if(ModelState.IsValid)
            {
                try
                {
                    if (_funcionarioRepositorio.EditarFuncionario(funcionario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                } catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao atualizar, tente novamente! ");
                }
            }
            return View(funcionario);
        }

        public IActionResult ExcluirFuncionario(int Id)
        {
            _funcionarioRepositorio.ExcluirFuncionario(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
