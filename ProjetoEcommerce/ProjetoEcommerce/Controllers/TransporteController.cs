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

        public IActionResult Index()
        {
            var lista = _transporteRepositorio.ListarTransportes();
            return View(lista);
        }

        public IActionResult CadastrarTransporte()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarTransporte(tbTransporte transporte)
        {
            if (_transporteRepositorio.CadastrarTransporte(transporte))
            {
                TempData["MensagemSucesso"] = "Transporte cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensagemErro"] = "Erro ao cadastrar transporte.";
            return View(transporte);
        }

        public IActionResult EditarTransporte(int id)
        {
            var transporte = _transporteRepositorio.ObterTransporte(id);
            if (transporte == null)
            {
                return NotFound();
            }
            return View(transporte);
        }

        [HttpPost]
        public IActionResult EditarTransporte(int id, [Bind("IdTransporte, CodigoTransporte, Companhia, TipoTransporte")] tbTransporte transporte)
        {
            ModelState.Clear();

            if (id != transporte.IdTransporte)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (_transporteRepositorio.AtualizarTransporte(transporte))
                {
                    TempData["MensagemSucesso"] = "Transporte atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["MensagemErro"] = "Erro ao atualizar transporte.";
                return View(transporte);
            }

            return View();
        }

        public IActionResult ExcluirTransporte(int id)
        {
            if (_transporteRepositorio.ExcluirTransporte(id))
            {
                TempData["MensagemSucesso"] = "Transporte excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao excluir transporte.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
