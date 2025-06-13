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
            if (await _passagemRepositorio.CadastrarPassagemAsync(passagem))
            {
                TempData["MensagemSucesso"] = "Passagem cadastrada com Sucesso";
                return RedirectToAction(nameof(Index));
            }
            TempData["MensagemErro"] = "Erro ao cadastrar passagem";
            return View(passagem);
        }

        public async Task<IActionResult> EditarPassagem(int id)
        {
            var passagem = await _passagemRepositorio.ObterPassagemAsync(id);

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
                if (await _passagemRepositorio.AtualizarPassagemAsync(passagem))
                {
                    TempData["MensagemSucesso"] = "Passagem atualizada com Sucesso";
                    return RedirectToAction(nameof(Index));
                }
                TempData["MensagemErro"] = "Erro ao atualizar passagem";
                return View(passagem);
            }

            return View();
        }

        public async Task<IActionResult> ExcluirPassagem(int id)
        {
            await _passagemRepositorio.ExcluirPassagemAsync(id);
            TempData["MensagemSucesso"] = "Passagem excluída com Sucesso";
            return RedirectToAction("Index", "PassagemComViagem");
        }

        public IActionResult ComprarPassagem(int id)
        {
            return View();
        }
    }
}
