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
                ViewData["MensagemSucesso"] = "Passagem cadastrada com Sucesso";
                return RedirectToAction(nameof(Index));  
            }
            ViewData["MensagemErro"] = "Erro ao cadastrar passagem";
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
                    ViewData["MensagemSucesso"] = "Passagem atualizada com Sucesso";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["MensagemErro"] = "Erro ao atualizar passagem";
                return View(passagem);
            }
                
            return View();
        }
        public IActionResult ExcluirPassagem(int id)
        {
            ViewData["MensagemSucesso"] = "Passagem excluida com Sucesso";
            _passagemRepositorio.ExcluirPassagem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
