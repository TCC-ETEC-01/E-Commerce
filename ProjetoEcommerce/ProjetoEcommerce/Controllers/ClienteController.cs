using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }
        public async Task<IActionResult> Index(string nome)
        {
            IEnumerable<tbCliente> clientes;

            if (string.IsNullOrEmpty(nome))
            {
                clientes = await _clienteRepositorio.TodosClientes();
            }
            else
            {
                clientes = await _clienteRepositorio.BuscarCliente(nome);
            }

            ViewBag.Nome = nome;
            return View(clientes);
        }


        public IActionResult CadastrarCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(tbCliente cliente)
        {
            if (!cliente.Cpf.All(char.IsDigit) || !cliente.Telefone.All(char.IsDigit))
            {
                TempData["MensagemErro"] = "Nos campos CPF e Telefone são aceitos apenas números. Digite novamente!";
                return View();
            }

            await _clienteRepositorio.CadastrarCliente(cliente);
            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditarCliente(int Id)
        {
            var cliente = await _clienteRepositorio.ObterCliente(Id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(int Id, [Bind("IdCliente,Nome,Sexo,Email,Telefone,Cpf")] tbCliente cliente)
        {
            if (Id != cliente.IdCliente)
            {
                return BadRequest();
            }
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    var atualizado = await _clienteRepositorio.EditarCliente(cliente);
                    if (atualizado)
                    {
                        TempData["MensagemSucesso"] = "Cliente atualizado com sucesso";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao atualizar, tente novamente!");
                }
            }

            return View(cliente);
        }

        public async Task<IActionResult> ExcluirCliente(int Id)
        {
            await _clienteRepositorio.ExcluirCliente(Id);
            TempData["MensagemSucesso"] = "Cliente excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }
    }
}
