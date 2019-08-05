using ControleFinanceiro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    public class ContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public ContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.ListaContaBanco = new ContaModel(HttpContextAccessor).ListaConta();
            return View();
        }

        [HttpPost]
        public IActionResult CriarConta(ContaModel formulario)
        {
            if (ModelState.IsValid)
            {
                formulario.HttpContextAccessor = HttpContextAccessor;
                formulario.inserirConta();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult CriarConta()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirConta(int id)
        {
            ContaModel objContaModel = new ContaModel(HttpContextAccessor);
            objContaModel.ExcluirConta(id);
            return RedirectToAction("Index");
        }
    }
}