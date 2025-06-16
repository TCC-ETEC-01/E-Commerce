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

            ViewBag.Termo = termo;
            return View(viagens);
        }

        public IActionResult CadastrarViagem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarViagem(tbViagem viagem)
        {
            var viagemCadastro = viagem.IdViagem;

            if (viagem == null)
            {
                ModelState.AddModelError("", "Dados da viagem s�o obrigatorios, tente novamente!");
            }
            else
            {
                if (!viagem.DataPartida.HasValue || !viagem.DataRetorno.HasValue)
                {
                    ModelState.AddModelError("", "Data de partida e retorno s�o obrigatorias, tente novamente!");
                }
                else
                {
                    if (viagem.DataPartida.Value.Year < 2025 || viagem.DataRetorno.Value.Year < 2025)
                    {
                        ModelState.AddModelError("", "Datas n�o podem ser anteriores ao ano de 2025");
                    }
                    if (viagem.DataRetorno <= viagem.DataPartida)
                    {
                        ModelState.AddModelError("", "A data de retorno precisa ser ap�s a data de partida!");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return View(viagem);
            }

            await _viagemRepositorio.CadastrarViagem(viagem);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditarViagem()
        {
            return View();
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
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar atualizar, tente novamente!");
                    return View(viagem);
                }
            }

            return View(viagem);
        }

        public async Task<IActionResult> ExcluirViagem(int Id)
        {
            await _viagemRepositorio.ExcluirViagem(Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BarraPesquisa()
        {
            return View();
        }
    }
}
