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
