using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemController : Controller
    {
        private readonly PassagemRepositorio _passagemRepositorio;
        
        public PassagemController(PassagemRepositorio passagemRepositorio)
        {
            _passagemRepositorio = passagemRepositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarPassagem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarPassagem(tbPassagem passagem)
        {
            return View(_pass);
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
