using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
namespace ProjetoEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastrarCliente(tbCliente cliente)
        {       
                //verifica se é possivel converter o valor para numérico, 
                //out _ é o retorno do parametro, nesse caso está sendo indicado que não tem a necessidade do retorno ser especificado.
                if (!int.TryParse(cliente.Cpf, out _))
                {
                    Console.WriteLine("Apenas números");
                }
            
            return View();
        }

        public IActionResult EditarFuncionario()
        {
            return View();
        }

        public IActionResult ExcluirFuncionario()
        {
            return View();
        }
    }
}
