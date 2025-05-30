using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class PacoteController : Controller
    {
        private readonly PacoteRepositorio _PacoteRepositorio;

        public PacoteController(PacoteRepositorio pacoteRepositorio)
        {
            _PacoteRepositorio = pacoteRepositorio;
        }

        public IActionResult Index()
        {
            return View(_PacoteRepositorio.TodosPacotes);
        }

        public IActionResult CadastrarPacote()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarPacote(tbPacote pacote)
        {
            if(_PacoteRepositorio.CadastrarPacote(pacote))
            {
                _PacoteRepositorio.CadastrarPacote(pacote);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult EditarPacote(int id)
        {
            var pacote = _PacoteRepositorio.ObterPacote(id);

            if (pacote == null)
            {
                return NotFound();
            }
            return View(pacote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarPacote(int id, [Bind("IdPacote,IdProduto, IdPassagem, NomePacote, Descricao, Valor,")]tbPacote pacote)
        { 
            ModelState.Clear();
            if (id != pacote.IdPacote)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            {
                _PacoteRepositorio.AtualizarPacote(pacote);
                return RedirectToAction(nameof(Index));
            }
            return View(pacote);
        }
        public IActionResult ExcluirPacote(int id)
        {
            _PacoteRepositorio.ExcluirPacote(id);
            return RedirectToAction(nameof(Index)); 
        }

    }
}
