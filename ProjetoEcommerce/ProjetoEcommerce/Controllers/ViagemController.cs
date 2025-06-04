using Microsoft.AspNetCore.Mvc;
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
    }
}
