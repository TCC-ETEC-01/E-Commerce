﻿using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;

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
        public async Task<IActionResult> CadastrarPassagem(tbPassagem passagem, int idViagem, int idTransporte)
        {
            passagem.IdViagem = new tbViagem { IdViagem = idViagem };
            passagem.IdTransporte = new tbTransporte { IdTransporte = idTransporte };

            if (await _passagemRepositorio.CadastrarPassagem(passagem))
            {
                TempData["MensagemSucesso"] = "Passagem cadastrada com sucesso";
                return RedirectToAction("Index", "PassagemComViagem");
            }

            ViewData["MensagemErro"] = "Erro ao cadastrar passagem";
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
        public async Task<IActionResult> EditarPassagem(int id, [Bind("IdPassagem,IdTransporte,Valor,Assento,IdViagem,Situacao,Translado")] tbPassagem passagem)
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
                    TempData["MensagemSucesso"] = "Passagem atualizada com sucesso";
                    return RedirectToAction("Index", "PassagemComViagem");
                }

                ViewData["MensagemErro"] = "Erro ao atualizar passagem";
                return View(passagem);
            }

            return View(passagem);
        }

        public async Task<IActionResult> ExcluirPassagem(int id)
        {
            if (await _passagemRepositorio.ExcluirPassagem(id))
            {
                TempData["MensagemSucesso"] = "Passagem excluída com sucesso";
                return RedirectToAction("Index", "PassagemComViagem");
            }
            TempData["MensagemErro"] = "Passagem já vendida!";

            return RedirectToAction("Index", "PassagemComViagem");
        }

        public async Task<IActionResult> ComprarPassagem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ComprarPassagem(tbVenda venda, int idPassagem, int idFuncionario, int idCliente)
        {
            venda.IdPassagem = new tbPassagem { IdPassagem = idPassagem };
            venda.IdFuncionario = new tbFuncionario { IdFuncionario = idFuncionario };
            venda.IdCliente = new tbCliente { IdCliente = idCliente };
                if (await _passagemRepositorio.VendaPassagem(venda))
                {
                    TempData["MensagemSucesso"] = "Venda cadastrada com sucesso";
                    return RedirectToAction("Index", "PassagemComViagem");
                }
            

            ViewData["MensagemErro"] = "Erro ao cadastrar venda";
            return View(venda);
        }
    }
}
