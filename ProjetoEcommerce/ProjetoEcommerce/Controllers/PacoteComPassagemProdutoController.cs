using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
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

        public async Task<IActionResult> Index(string nomePacote)
        {
            IEnumerable<tbPacoteComPassagemProduto> pacotes;
            if (string.IsNullOrEmpty(nomePacote))
            {
                pacotes = await _pacoteComPassagemProdutoRepositorio.PacoteComPassagemProduto();
            }
            else 
            {
                pacotes = await _pacoteComPassagemProdutoRepositorio.BuscarPacote(nomePacote);
            }
            ViewBag.Nome = nomePacote;
            return View(pacotes);
        }
        
    }
}
