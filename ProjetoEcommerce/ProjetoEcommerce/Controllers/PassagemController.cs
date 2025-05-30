using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarPassagem()
        {
            return View();
        }

        public IActionResult EditarPassagem()
        {
            return View();
        }
        public IActionResult ExcluirPassagem()
        {
            return View();
        }
    }
}
