﻿using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult CadastrarFuncionario()
        {
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
