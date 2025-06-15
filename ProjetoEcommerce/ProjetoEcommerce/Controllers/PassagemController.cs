using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;
using ProjetoEcommerce.Controllers;
using System;

namespace ProjetoEcommerce.Controllers
{
    public class PassagemController : Controller
    {
        private readonly PassagemRepositorio _passagemRepositorio;

        public PassagemController(PassagemRepositorio passagemRepositorio)
        {
            _passagemRepositorio = passagemRepositorio;
        }

        public IActionResult CadastrarPassagem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPassagem(tbPassagem passagem)
        {
            if (await _passagemRepositorio.CadastrarPassagem(passagem))
            {
                TempData["MensagemSucesso"] = "Passagem cadastrada com Sucesso";
                return RedirectToAction("Index", "PassagemComViagem");
            }
            TempData["MensagemErro"] = "Erro ao cadastrar passagem";
            return View(passagem);
        }

        public async Task<IActionResult> EditarPassagem(int id)
        {
            var passagem = await _passagemRepositorio.ObterPassagem(id);

            if (passagem == null)
            {
                return NotFound();
            }
            return View(passagem);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPassagem(int id, [Bind("IdPassagem, Valor, Assento, IdViagem, Situacao")] tbPassagem passagem)
        {
            ModelState.Clear();
            if (id != passagem.IdPassagem)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (await _passagemRepositorio.AtualizarPassagem(passagem))
                {
                    TempData["MensagemSucesso"] = "Passagem atualizada com Sucesso";
                    return RedirectToAction("Index", "PassagemComViagem");
                }
                TempData["MensagemErro"] = "Erro ao atualizar passagem";
                return View(passagem);
            }

            return View();
        }

        public async Task<IActionResult> ExcluirPassagem(int id)
        {
            await _passagemRepositorio.ExcluirPassagem(id);
            TempData["MensagemSucesso"] = "Passagem excluída com Sucesso";
            return RedirectToAction("Index", "PassagemComViagem");
        }

        public async Task<IActionResult> ComprarPassagem(tbVenda venda)
        {
            if ( await _passagemRepositorio.VendaPassagem(venda))
            {
                TempData["MensagemSucesso"] = "Venda cadastrada com sucesso";
                return RedirectToAction("Index", "PassagemComViagem");

            }
            TempData["MensagemErro"] = "Erro ao cadastrar venda";
            return View(venda);
        }
    }
}
