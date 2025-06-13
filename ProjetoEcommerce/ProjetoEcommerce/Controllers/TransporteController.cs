﻿using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorios;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Controllers
{
    public class TransporteController : Controller
    {
        private readonly TransporteRepositorio _transporteRepositorio;

        public TransporteController(TransporteRepositorio transporteRepositorio)
        {
            _transporteRepositorio = transporteRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _transporteRepositorio.ListarTransportes();
            return View(lista);
        }

        public IActionResult CadastrarTransporte()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarTransporte(tbTransporte transporte)
        {
            if (await _transporteRepositorio.CadastrarTransporte(transporte))
            {
                TempData["MensagemSucesso"] = "Transporte cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensagemErro"] = "Erro ao cadastrar transporte.";
            return View(transporte);
        }

        public async Task<IActionResult> EditarTransporte(int id)
        {
            var transporte = await _transporteRepositorio.ObterTransporte(id);
            if (transporte == null)
            {
                return NotFound();
            }
            return View(transporte);
        }

        [HttpPost]
        public async Task<IActionResult> EditarTransporte(int id, [Bind("IdTransporte, CodigoTransporte, Companhia, TipoTransporte")] tbTransporte transporte)
        {
            ModelState.Clear();

            if (id != transporte.IdTransporte)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (await _transporteRepositorio.AtualizarTransporte(transporte))
                {
                    TempData["MensagemSucesso"] = "Transporte atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["MensagemErro"] = "Erro ao atualizar transporte.";
                return View(transporte);
            }

            return View();
        }

        public async Task<IActionResult> ExcluirTransporte(int id)
        {
            if (await _transporteRepositorio.ExcluirTransporte(id))
            {
                TempData["MensagemSucesso"] = "Transporte excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao excluir transporte.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
