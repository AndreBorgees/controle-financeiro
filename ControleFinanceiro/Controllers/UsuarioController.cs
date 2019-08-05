using ControleFinanceiro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Login(int? id)
        {
            if (id != null)
            {
                if (id == 0)
                {
                    HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult validarLogin(UsuarioModel usuario)
        {

            bool login = usuario.validarLogin();
            if (login)
            {
                HttpContext.Session.SetString("IdUsuarioLogado", usuario.Id.ToString());
                HttpContext.Session.SetString("NomeUsuarioLogado", usuario.Nome);
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                TempData["MensagemLoginInvalido"] = "Login Invalido";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.RegistrarUsuario();
                return RedirectToAction("Sucesso");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Sucesso()
        {
            return View();
        }

    }
}