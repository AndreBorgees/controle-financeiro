using ControleFinanceiro.Util;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace ControleFinanceiro.Models
{
    public class ContaModel
    {
        public int id_conta { get; set; }
        [Required(ErrorMessage = "Informe o nome da conta")]
        public string nome_conta { get; set; }
        [Required(ErrorMessage = "Informe o saldo da conta")]
        public double valor_saldo { get; set; }
        public int id_usuario { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ContaModel() { }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public ContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<ContaModel> ListaConta()
        {

            List<ContaModel> lista = new List<ContaModel>();
            ContaModel item;

            string sql = $"select Id, Nome, Saldo, Usuario_Id from conta where Usuario_Id = {IdUsuarioLogado()}";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ContaModel();
                item.id_conta = (int)dt.Rows[i]["Id"];
                item.nome_conta = dt.Rows[i]["Nome"].ToString();
                item.valor_saldo = double.Parse(dt.Rows[i]["Saldo"].ToString());
                item.id_usuario = (int)dt.Rows[i]["Usuario_Id"];
                lista.Add(item);
            }

            return lista;
        }
        public void inserirConta()
        {
            DAL objDAL = new DAL();
            string sql = $"insert into conta (Nome, Saldo, Usuario_Id) values ('{nome_conta}','{valor_saldo}','{IdUsuarioLogado()}')";
            objDAL.executaSQL(sql);
        }

        public void ExcluirConta(int id_conta)
        {
            new DAL().executaSQL("delete from conta where Id = " + id_conta); 
        }
    }


}
