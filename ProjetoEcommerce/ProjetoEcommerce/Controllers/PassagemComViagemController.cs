using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemComViagemController : Controller
    {
        private readonly PassagemComViagemRepositorio _passagemComViagemRepositorio;
        public PassagemComViagemController(PassagemComViagemRepositorio passagemComViagemRepositorio)
        {
            _passagemComViagemRepositorio = passagemComViagemRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var passagensComViagem = await _passagemComViagemRepositorio.PassagemComViagem();
            return View(passagensComViagem);
        }

        public async Task<IActionResult> BarraPesquisa()
        {
            return View();
        }
    }
}
