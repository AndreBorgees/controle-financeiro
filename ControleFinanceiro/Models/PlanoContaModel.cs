

using ControleFinanceiro.Util;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ControleFinanceiro.Models
{
    public class PlanoContaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a Descrição!")]
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int Usuario_Id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public PlanoContaModel() { }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public PlanoContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<PlanoContaModel> ListaPlanoConta()
        {

            List<PlanoContaModel> lista = new List<PlanoContaModel>();
            PlanoContaModel item;

            string sql = $"select Id, Descricao, Tipo, Usuario_Id from plano_contas where Usuario_Id = {IdUsuarioLogado()}";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new PlanoContaModel();
                item.Id = (int)dt.Rows[i]["Id"];
                item.Descricao = dt.Rows[i]["Descricao"].ToString();
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Usuario_Id = (int)dt.Rows[i]["Usuario_Id"];
                lista.Add(item);
            }

            return lista;
        }

        public PlanoContaModel CarregaRegistro(int? id)
        {

            PlanoContaModel item = new PlanoContaModel();

            string sql = $"select Id, Descricao, Tipo, Usuario_Id from plano_contas where Usuario_Id = {IdUsuarioLogado()} and id = {id}";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);

            item.Id = (int)dt.Rows[0]["Id"];
            item.Descricao = dt.Rows[0]["Descricao"].ToString();
            item.Tipo = dt.Rows[0]["Tipo"].ToString();
            item.Usuario_Id = (int)dt.Rows[0]["Usuario_Id"];

            return item;
        }


        public void inserirPlanoConta()
        {
            DAL objDAL = new DAL();
     
            string sql;
            if (Id == 0)
            {
                sql = $"insert into plano_contas (Descricao, Tipo, Usuario_Id) values ('{Descricao}','{Tipo}','{IdUsuarioLogado()}')";

            }
            else
            {
                sql = $"update plano_contas set Descricao = '{Descricao}', Tipo = '{Tipo}' where Usuario_id = '{IdUsuarioLogado()}' and id = '{Id}'";
            }
            objDAL.executaSQL(sql);
        }

        public void ExcluirPlanoConta(int Id)
        {
            new DAL().executaSQL("delete from plano_contas where Id = " + Id);
        }
    }
}
