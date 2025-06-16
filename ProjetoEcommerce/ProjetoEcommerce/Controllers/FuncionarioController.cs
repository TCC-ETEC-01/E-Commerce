using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioController(FuncionarioRepositorio funcionario)
        {
            _funcionarioRepositorio = funcionario;
        }

        public async Task<IActionResult> Index(string nome)
        {
            IEnumerable<tbFuncionario> funcionarios;

            if(string.IsNullOrEmpty(nome))
            {
                funcionarios = await _funcionarioRepositorio.TodosFuncionarios();
            }
            else
            {
                funcionarios = await _funcionarioRepositorio.BuscarFuncionario(nome);
            }
            ViewBag.Nome = nome;
            return View(funcionarios);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var funcionario = await _funcionarioRepositorio.ObterFuncionarioEmail(email);
            if (funcionario != null && funcionario.Senha == senha)
            {
                HttpContext.Session.SetString("FuncionarioLogado", funcionario.Email);
                TempData["Mensagem"] = "Bem vindo" + funcionario.Email;
                return RedirectToAction("Index", "DashBoard");
            }
            ViewBag.Erro = "Dados incorretos, tente novamente!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mesangem"] = "Você está indo embora? Até breve!";
            return RedirectToAction("Login", "Funcionario");
        }

        public IActionResult CadastrarFuncionario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarFuncionario(tbFuncionario funcionario)
        {
            if (!funcionario.Cpf.All(char.IsDigit) || !funcionario.Telefone.All(char.IsDigit))
            {
                TempData["MensagemErro"] = "No campo CPF  e Telefone apenas numeros!";
                return View();
            }

            var sucesso = await _funcionarioRepositorio.CadastrarFuncionario(funcionario);
            if (sucesso)
            {
                TempData["MensagemErro"] = "Cadastro Realizado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao cadastrar o funcionário.";
                return View();
            }
        }

        public async Task<IActionResult> EditarFuncionario(int Id)
        {
            var funcionario = await _funcionarioRepositorio.ObterFuncionarioID(Id);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarFuncionario(int Id, [Bind("IdFuncionario,Nome,Sexo,Email,Telefone,Cargo,Cpf,Senha")] tbFuncionario funcionario)
        {
            if (Id != funcionario.IdFuncionario)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _funcionarioRepositorio.EditarFuncionario(funcionario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao atualizar, tente novamente! ");
                }
            }
            return View(funcionario);
        }

        public async Task<IActionResult> ExcluirFuncionario(int Id)
        {
            await _funcionarioRepositorio.ExcluirFuncionario(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
