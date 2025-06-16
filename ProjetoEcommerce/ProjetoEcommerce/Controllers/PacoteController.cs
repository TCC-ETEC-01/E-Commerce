using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Controllers
{
    public class PacoteController : Controller
    {
        private readonly PacoteRepositorio _PacoteRepositorio;

        public PacoteController(PacoteRepositorio pacoteRepositorio)
        {
            _PacoteRepositorio = pacoteRepositorio;
        }

        public IActionResult CadastrarPacote()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPacote(tbPacote pacote, int idPassagem, int idProduto)
        {
           pacote.IdPassagem = new tbPassagem { IdPassagem = idPassagem};
            pacote.IdProduto= new tbProduto{ IdProduto = idProduto };
            if (await _PacoteRepositorio.CadastrarPacote(pacote))
            {
                TempData["MensagemSucesso"] = "Pacote cadastrado com Sucesso";
                return RedirectToAction("Index", "PacoteComPassagemProduto");
            }

            TempData["MensagemErro"] = "Erro ao cadastrar pacote";
            return View();
        }

        public async Task<IActionResult> EditarPacote(int id)
        {
            var pacote = await _PacoteRepositorio.ObterPacote(id);

            if (pacote == null)
            {
                return NotFound();
            }

            return View(pacote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPacote(int id, [Bind("IdPacote,IdProduto,IdPassagem,NomePacote,Descricao,Valor")] tbPacote pacote)
        {
            ModelState.Clear();

            if (id != pacote.IdPacote)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _PacoteRepositorio.AtualizarPacote(pacote);
                TempData["MensagemSucesso"] = "Pacote atualizado com Sucesso";
                return RedirectToAction("Index", "PacoteComPassagemProduto");
            }

            TempData["MensagemErro"] = "Erro ao atualizar pacote";
            return View(pacote);
        }

        public async Task<IActionResult> ExcluirPacote(int id)
        {
            await _PacoteRepositorio.ExcluirPacote(id);
            TempData["MensagemSucesso"] = "Pacote excluído com sucesso";
            return RedirectToAction("Index", "PacoteComPassagemProduto");
        }
    }
}
