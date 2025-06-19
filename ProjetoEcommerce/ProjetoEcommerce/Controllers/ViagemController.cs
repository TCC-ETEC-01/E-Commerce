using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

namespace ProjetoEcommerce.Controllers
{
    public class ViagemController : Controller
    {
        private readonly ViagemRepositorio _viagemRepositorio;

        public ViagemController(ViagemRepositorio viagemRepositorio)
        {
            _viagemRepositorio = viagemRepositorio;
        }

        public async Task<IActionResult> Index(string termo)
        {
            IEnumerable<tbViagem> viagens;

            if (string.IsNullOrEmpty(termo))
            {
                viagens = await _viagemRepositorio.TodasViagens();
            }
            else
            {
                viagens = await _viagemRepositorio.BuscarViagens(termo);
            }

            ViewData["Termo"] = termo;
            return View(viagens);
        }

        public IActionResult CadastrarViagem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarViagem(tbViagem viagem)
        {
            if (viagem == null)
            {
                ModelState.AddModelError("", "Dados da viagem são obrigatórios, tente novamente!");
            }
            else
            {
                if (!viagem.DataPartida.HasValue || !viagem.DataRetorno.HasValue)
                {
                    ModelState.AddModelError("", "Data de partida e retorno são obrigatórias, tente novamente!");
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
                return View(viagem);
            }

            await _viagemRepositorio.CadastrarViagem(viagem);
            TempData["MensagemSucesso"] = "Viagem cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditarViagem(int id)
        {
            var viagem = await _viagemRepositorio.ObterViagem(id);

            if (viagem == null)
            {
                return NotFound();
            }
            return View(viagem);
        }

        [HttpPost]
        public async Task<IActionResult> EditarViagem(int Id, [Bind("IdViagem,DataRetorno,Descricao,Origem,Destino,TipoTransporte,DataPartida")] tbViagem viagem)
        {
            if (Id != viagem.IdViagem)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _viagemRepositorio.EditarViagem(viagem))
                    {
                        TempData["MensagemSucesso"] = "Viagem atualizada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar atualizar, tente novamente!");
                }
            }

            return View(viagem);
        }

        public async Task<IActionResult> ExcluirViagem(int Id)
        {
            if (await _viagemRepositorio.ExcluirViagem(Id))
            {
       
                TempData["MensagemSucesso"] = "Viagem excluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Passagem já vendida!";

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> BarraPesquisa()
        {
            return View();
        }
    }
}
