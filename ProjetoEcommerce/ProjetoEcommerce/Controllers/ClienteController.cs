using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarCliente()
        {
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
