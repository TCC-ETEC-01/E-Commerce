using Microsoft.AspNetCore.Mvc;
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

            if (string.IsNullOrEmpty(nome))
            {
                funcionarios = await _funcionarioRepositorio.TodosFuncionarios();
            }
            else
            {
                funcionarios = await _funcionarioRepositorio.BuscarFuncionario(nome);
            }

            ViewData["Nome"] = nome;
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
                TempData["MensagemSucesso"] = "Bem-vindo, " + funcionario.Nome;
                return RedirectToAction("Index", "DashBoard");
            }

            ViewData["MensagemErro"] = "Dados incorretos, tente novamente!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["MensagemSucesso"] = "Você está indo embora? Até breve!";
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
                ViewData["MensagemErro"] = "Nos campos CPF e Telefone, apenas números são aceitos.";
                return View();
            }

            var sucesso = await _funcionarioRepositorio.CadastrarFuncionario(funcionario);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MensagemErro"] = "Erro ao cadastrar o funcionário.";
            return View();
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


                        TempData["MensagemSucesso"] = "Funcionário editado com sucesso";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ViewData["MensagemErro"] = "Ocorreu um erro ao atualizar, tente novamente!";
                }
            }

            return View(funcionario);
        }

        public async Task<IActionResult> ExcluirFuncionario(int Id)
        {
            var funcionario = await _funcionarioRepositorio.ObterFuncionarioID(Id);
            if (funcionario == null)
            {
                return NotFound();
            }

            await _funcionarioRepositorio.ExcluirFuncionario(Id);

            var funcionarioLogado = HttpContext.Session.GetString("FuncionarioLogado") ?? "Desconhecido";

            await _funcionarioRepositorio.RegistrarLog(
                funcionarioLogado,
                "Excluir",
                $"Funcionário ID {Id} - {funcionario.Nome} excluído."
            );

            TempData["MensagemSucesso"] = "Funcionário excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }
    }
}
