using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
        
        {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }
        public IActionResult Index()
        {
            return View(_produtoRepositorio.TodosProdutos);
        }

        public IActionResult CadastrarProduto()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarProduto(tbProduto produto)
        {
            if (ModelState.IsValid)
            {
                _produtoRepositorio.CadastrarProduto(produto);
                return RedirectToAction(nameof(Index));
            }
                return View();
        }

        public IActionResult EditarProduto()
        {
            return View();
        }

        public IActionResult ExcluirProduto()
        {
            return View();
        }

    }
}

