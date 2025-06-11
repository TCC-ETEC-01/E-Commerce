using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepositorio.TodosProdutos();
            return View(produtos);
        }

        public IActionResult CadastrarProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProduto(tbProduto produto)
        {
            if (await _produtoRepositorio.CadastrarProduto(produto))
            {
                TempData["MensagemSucesso"] = "Produto cadastrado com Sucesso";
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Erro ao cadastrar produto";
            return View();
        }

        public async Task<IActionResult> EditarProduto(int id)
        {
            var produto = await _produtoRepositorio.ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProduto(int id, [Bind("IdProduto,Valor,NomeProduto,Descricao,Quantidade")] tbProduto produto)
        {
            ModelState.Clear();
            if (id != produto.IdProduto)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                if (await _produtoRepositorio.AtualizarProduto(produto))
                {
                    TempData["MensagemSucesso"] = "Produto cadastrado com Sucesso";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["MensagemErro"] = "Erro ao atualizar produto";
            return View(produto);
        }

        public async Task<IActionResult> ExcluirProduto(int id)
        {
            await _produtoRepositorio.ExcluirProduto(id);
            TempData["MensagemSucesso"] = "Produto excluido com sucesso";
            return RedirectToAction(nameof(Index));
        }
    }
}
