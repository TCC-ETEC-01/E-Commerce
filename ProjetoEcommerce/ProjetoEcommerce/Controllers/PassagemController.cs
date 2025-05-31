using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemController : Controller
    {
        private readonly PassagemRepositorio _passagemRepositorio;
        
        public PassagemController(PassagemRepositorio passagemRepositorio)
        {
            _passagemRepositorio = passagemRepositorio;
        }
        public IActionResult Index()
        {
            return View(_passagemRepositorio.TodasPassagens);
        }

        public IActionResult CadastrarPassagem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarPassagem(tbPassagem passagem)
        {
           if(_passagemRepositorio.CadastrarPassagem(passagem))
            {
                _passagemRepositorio.CadastrarPassagem(passagem);
                 return RedirectToAction(nameof(Index));  
            }
           return View();
        }
        public IActionResult EditarPassagem(int id)
        {
            var passagem = _passagemRepositorio.ObterPassagem(id);

            if (passagem == null)
            {
                return NotFound();
            }
            return View(passagem);
        }

        [HttpPost]
        public IActionResult EditarPassagem(int id, [Bind("IdPassagem, Valor, Assento, IdViagem, Situacao")] tbPassagem passagem)
        {
            ModelState.Clear();
            if(id != passagem.IdPassagem)
            {
                return BadRequest();
            }
                if(ModelState.IsValid)
            {
                if(_passagemRepositorio.AtualizarPassagem(passagem))
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(passagem);
            }
                
            return View();
        }
        public IActionResult ExcluirPassagem()
        {
            return View();
        }
    }
}
