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
                TempData["MensagemSucesso"] = "Produto cadastrado com Sucesso";
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Erro ao cadastrar produto";
            return View();
        }

        public IActionResult EditarProduto(int id)
        {
            var produto = _produtoRepositorio.ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        public IActionResult EditarProduto(int id, [Bind("IdProduto,Valor,NomeProduto,Descricao,Quantidade")] tbProduto produto)
        {
            ModelState.Clear();
            if (id != produto.IdProduto)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                if (_produtoRepositorio.AtualizarProduto(produto))
                {
                    TempData["MensagemSucesso"] = "Produto cadastrado com Sucesso";
                    return RedirectToAction(nameof(Index));
                }
            }
             
            TempData["MensagemErro"] = "Erro ao atualizar produto";
            return View(produto);
        }

        public IActionResult ExcluirProduto(int id)
        {
            _produtoRepositorio.ExcluirProduto(id);
            TempData["MensagemSucesso"] = "Produto excluido com sucesso";
            return RedirectToAction(nameof(Index));
        }

    }
}

