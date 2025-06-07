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

        public IActionResult Index()
        {
            return View();
        }
    }
}
