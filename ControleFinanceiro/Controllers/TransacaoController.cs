
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ControleFinanceiro.Models;
using ControleFinanceiro.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    public class TransacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.ListaTransacao = new TransacaoModel(HttpContextAccessor).ListaTransacao();
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(TransacaoModel formulario)
        {
            if (ModelState.IsValid)
            {
                formulario.HttpContextAccessor = HttpContextAccessor;
                formulario.inserirTransacao();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            if (id != null)
            {
                ViewBag.ListaTransacao = new TransacaoModel(HttpContextAccessor).CarregaRegistro(id);
            }
            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta();
            ViewBag.ListaPlanoContas = new PlanoContaModel(HttpContextAccessor).ListaPlanoConta();
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {

            ViewBag.ListaTransacao = new TransacaoModel(HttpContextAccessor).CarregaRegistro(id);
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            new TransacaoModel(HttpContextAccessor).ExcluirTransacao(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Extrato(TransacaoModel formulario)
        {
            formulario.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListaTransacao = formulario.ListaTransacao();
            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta();
            return View();
        }

        public IActionResult Dashboard()
        {


            List<Dashboard> lista = new Dashboard(HttpContextAccessor).RetornaGrafico();

            string valores = "";
            string labels = "";
            string cores = "";
            var random = new Random();
     

            for (int i = 0; i < lista.Count; i++)
            {
                valores += lista[i].Total.ToString() + ",";
                labels += "'" + lista[i].PlanoConta.ToString() + "',";
                cores += "'" + String.Format("#{0:X6}", random.Next(0x1000000)) + "',"; 
            }         
            ViewBag.Cores = cores;
            ViewBag.Labels = labels;
            ViewBag.Valores = valores;

            return View();
        }

    }


}