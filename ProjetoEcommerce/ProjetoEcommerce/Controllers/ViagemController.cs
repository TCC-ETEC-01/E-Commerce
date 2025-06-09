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

