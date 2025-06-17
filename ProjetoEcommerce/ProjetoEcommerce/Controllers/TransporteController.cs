using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class TransporteController : Controller
    {
        private readonly TransporteRepositorio _transporteRepositorio;

        public TransporteController(TransporteRepositorio transporteRepositorio)
        {
            _transporteRepositorio = transporteRepositorio;
        }

        public async Task<IActionResult> Index(string termo)
        {
            List<tbTransporte> transportes;

            if (string.IsNullOrEmpty(termo))
            {
                transportes = await _transporteRepositorio.ListarTransportes();
            }
            else
            {
                transportes = await _transporteRepositorio.BuscarTransporte(termo);
            }

            ViewBag.Termo = termo;
            return View(transportes);
        }

        public IActionResult CadastrarTransporte()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarTransporte(tbTransporte transporte)
        {
            if (await _transporteRepositorio.CadastrarTransporte(transporte))
            {
                TempData["MensagemSucesso"] = "Transporte cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensagemErro"] = "Erro ao cadastrar transporte.";
            return View(transporte);
        }

        public async Task<IActionResult> EditarTransporte(int id)
        {
            var transporte = await _transporteRepositorio.ObterTransporte(id);
            if (transporte == null)
            {
                return NotFound();
            }
            return View(transporte);
        }

        [HttpPost]
        public async Task<IActionResult> EditarTransporte(int id, [Bind("IdTransporte, CodigoTransporte, Companhia, TipoTransporte")] tbTransporte transporte)
        {
            ModelState.Clear();

            if (id != transporte.IdTransporte)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (await _transporteRepositorio.AtualizarTransporte(transporte))
                {
                    TempData["MensagemSucesso"] = "Transporte atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["MensagemErro"] = "Erro ao atualizar transporte.";
                return View(transporte);
            }

            return View();
        }

        public async Task<IActionResult> ExcluirTransporte(int id)
        {
            await _transporteRepositorio.ExcluirTransporte(id); 
           TempData["MensagemErro"] = "Erro ao excluir transporte.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BarraPesquisa()
        {
            return View();
        }
    }
}
