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

        public IActionResult Index()
        {
            return View(_viagemRepositorio.TodasViagens());
        }
        public IActionResult CadastrarViagem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarViagem(tbViagem viagem)
        {
            var viagemCadastro = viagem.IdViagem;
            if (viagem == null)
            {
                ModelState.AddModelError("", "Dados da viagem são obrigatorios, tente novamente!");
            }
            else
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
            _viagemRepositorio.CadastrarViagem(viagem);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EditarViagem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditarViagem(int Id, [Bind("IdViagem,DataRetorno,Descricao,Origem,Destino,TipoTransporte,DataPartida")]tbViagem viagem)
        {
            if (Id !=viagem.IdViagem)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (_viagemRepositorio.EditarViagem(viagem))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar atualizar, tente novamente!");
                    return View(viagem);
                }
            }
            return View(viagem);
        }

        public IActionResult ExcluirViagem(int Id)
        {
            _viagemRepositorio.ExcluirViagem(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}

