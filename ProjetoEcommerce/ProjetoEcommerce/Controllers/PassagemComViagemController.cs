using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemComViagemController : Controller
    {
        private readonly PassagemComViagemRepositorio _passagemComViagemRepositorio;
        public PassagemComViagemController(PassagemComViagemRepositorio passagemComViagemRepositorio)
        {
            _passagemComViagemRepositorio = passagemComViagemRepositorio;
        }

        public IActionResult Index()
        {
            return View(_passagemComViagemRepositorio.PassagemComViagem);
        }
    }
}
