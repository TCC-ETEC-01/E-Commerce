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
            var viagem = passagem.IdViagem;
            if(viagem == null)
            {
                ModelState.AddModelError("", "Dados da viagem são obrigatorios, tente novamente!");
            } else
            {
                if (!viagem.DataPartida.HasValue || !viagem.DataRetorno.HasValue)
                {
                    ModelState.AddModelError("", "Data de partida e retorno são obrigatorias, tente novamente!");
                }
                else
                {
                    if (viagem.DataPartida.Value.Year < 2025 || viagem.DataRetorno.Value.Year < 2025)
                    {
                        ModelState.AddModelError("", "Datas não podem ser anteriores ao ano de 2025");
                    } 
                    if (viagem.DataRetorno <= viagem.DataPartida)
                    {
                        ModelState.AddModelError("", "A data de retorno precisa ser após a data de partida!");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Erro ao cadastrar passagem, tente novamente";
                return View(passagem);
            }
            if(_passagemRepositorio.CadastrarPassagem(passagem))
            {
                TempData["MensagemSucesso"] = "Passagem cadastrada com Sucesso";
                return RedirectToAction(nameof(Index));  
            }
            TempData["MensagemErro"] = "Erro ao cadastrar passagem";
            return View(passagem);
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
                    TempData["MensagemSucesso"] = "Passagem atualizada com Sucesso";
                    return RedirectToAction(nameof(Index));
                }
                TempData["MensagemErro"] = "Erro ao atualizar passagem";
                return View(passagem);
            }
                
            return View();
        }
        public IActionResult ExcluirPassagem(int id)
        {
            TempData["MensagemSucesso"] = "Passagem excluida com Sucesso";
            _passagemRepositorio.ExcluirPassagem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
