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

        public IActionResult EditarCliente()
        {
            return View();
        }

        public IActionResult ExcluirCliente()
        {
            return View();
        }
    }
}
