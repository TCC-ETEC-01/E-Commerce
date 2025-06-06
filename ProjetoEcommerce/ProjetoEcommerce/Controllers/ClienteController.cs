using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
namespace ProjetoEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            return View(_clienteRepositorio.TodosClientes());
        }

        public IActionResult CadastrarCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarCliente(tbCliente cliente)
        {       
                //verifica se é possivel converter o valor para numérico, 
                //out _ é o retorno do parametro, nesse caso está sendo indicado que não tem a necessidade do retorno ser especificado.
                if (!int.TryParse(cliente.Cpf, out _) && !int.TryParse(cliente.Telefone, out _))
                {
<<<<<<< HEAD
                ViewData["MensagemErro"] = "No campo CPF e Telefone são aceitos apenas numeros, digite novamente!";
                }

            
            return RedirectToAction(nameof(Index));


=======
                TempData["MensagemErro"] = "No campo CPF e Telefone são aceitos apenas numeros, digite novamente!";
                }            
            return RedirectToAction(nameof(Index));
>>>>>>> CauaEstruturandoViews
        }
        
        public IActionResult EditarCliente(int Id)
        {
            var cliente = _clienteRepositorio.ObterCliente(Id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
        [HttpPost]
        public IActionResult EditarCliente(int Id, [Bind("IdCliente,Nome,Sexo,Email,Telefone,Cpf")]tbCliente cliente)
        {
            if(Id != cliente.IdCliente)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (_clienteRepositorio.EditarCliente(cliente))
                    {
                        TempData["MensagemSucesso"] = "Cliente atualizado com sucesso";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao atualizar, tente novamente! ");
                }
            }
            return View(cliente);
        }
        public IActionResult ExcluirCliente(int Id)
        {
            _clienteRepositorio.ExcluirCliente(Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
