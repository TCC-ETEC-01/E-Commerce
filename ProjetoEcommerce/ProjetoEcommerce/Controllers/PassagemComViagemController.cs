using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
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

        public async Task<IActionResult> Index(string destino)
        {
            IEnumerable<tbPassagemComViagem> passagens;

            if (string.IsNullOrEmpty(destino))
            {
                passagens = await _passagemComViagemRepositorio.PassagemComViagem();
            }
            else
            {
                passagens = await _passagemComViagemRepositorio.BuscarPassagem(destino);
            }

            ViewBag.Destino = destino;
            return View(passagens);
        }
    }
}
