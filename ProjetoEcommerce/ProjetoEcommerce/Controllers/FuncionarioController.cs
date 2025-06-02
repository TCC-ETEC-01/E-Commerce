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

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult CadastrarFuncionario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarFuncionario(tbFuncionario funcionario)
        {
            if (!int.TryParse(funcionario.Cpf, out _))
            {
                Console.WriteLine("No campo CPF, apenas números!");
            }
            return View();
        }

        public IActionResult EditarFuncionario()
        {
            return View();
        }

        public IActionResult ExcluirFuncionario()
        {
            return View();
        }
    }
}
