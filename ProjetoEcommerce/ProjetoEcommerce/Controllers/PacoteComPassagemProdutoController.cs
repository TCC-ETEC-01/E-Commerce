using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Repositorios;
namespace ProjetoEcommerce.Controllers
{
    public class PacoteComPassagemProdutoController : Controller
    {
        private readonly PacoteComPassagemProdutoRepositorio _pacoteComPassagemProdutoRepositorio;

        public PacoteComPassagemProdutoController(PacoteComPassagemProdutoRepositorio pacoteComPassagemProdutoRepositorio)
        {
            _pacoteComPassagemProdutoRepositorio = pacoteComPassagemProdutoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var pacotes = await _pacoteComPassagemProdutoRepositorio.PacoteComPassagemProduto();
            return View(pacotes);
        }
    }
}
