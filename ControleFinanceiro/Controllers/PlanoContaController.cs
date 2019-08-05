using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleFinanceiro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    public class PlanoContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public PlanoContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.ListaPlanoConta = new PlanoContaModel(HttpContextAccessor).ListaPlanoConta();
            return View();
        }

        [HttpPost]
        public IActionResult CriarPlanoConta(PlanoContaModel formulario)
        {
            if (ModelState.IsValid)
            {
                formulario.HttpContextAccessor = HttpContextAccessor;
                formulario.inserirPlanoConta();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult CriarPlanoConta(int? id)
        {
                if (id != null)
            {
                ViewBag.RegistroPlanoConta = new PlanoContaModel(HttpContextAccessor).CarregaRegistro(id);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirPlanoConta(int id)
        {
            PlanoContaModel objPanoContaModel = new PlanoContaModel(HttpContextAccessor);
            objPanoContaModel.ExcluirPlanoConta(id);
            return RedirectToAction("Index");
        }
    }
}