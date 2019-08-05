using ControleFinanceiro.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ControleFinanceiro.Models
{
    public class TransacaoModel
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public string DataFinal { get; set; }
        [Required(ErrorMessage = "Informe a Data")]
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string DescricaoPlanoConta { get; set; }
        public string Nome_conta { get; set; }
        public double Valor { get; set; }
        public int Conta_Id { get; set; }
        public int Plano_Contas_Id { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel() { }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<TransacaoModel> ListaTransacao()
        {

            List<TransacaoModel> lista = new List<TransacaoModel>();
            TransacaoModel item;

            //Filtro extrato
            string filtro = "";
            if ((Data != null) && (DataFinal != null))
            {
                filtro += $" and t.Data >='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' and t.Data <='{DateTime.Parse(DataFinal).ToString("yyyy/MM/dd")}' ";

            }

            if (Tipo != null)
            {
                if (Tipo != "A")
                {
                    filtro += $" and t.Tipo = '{Tipo}'";
                }
            }

            if (Conta_Id != 0)
            {

                filtro += $" and t.Conta_Id = '{Conta_Id}'";

            }


            //Fim

            string idUsuarioLogado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select t.id, t.Data, t.tipo, t.valor, t.descricao as historico, t.Conta_Id, c.nome, t.plano_contas_id, p.descricao as plano_conta "
                + " from transacao as t "
                + " inner join conta c on t.conta_id = c.id "
                + " inner join plano_contas as p on t.plano_contas_id = p.id"
                + $" where t.usuario_id = '{idUsuarioLogado}' {filtro} "
                + " order by t.data desc"
                + "  limit 10";


            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new TransacaoModel();
                item.Id = (int)dt.Rows[i]["Id"];
                item.Data = DateTime.Parse(dt.Rows[i]["Data"].ToString()).ToString("dd/MM/yyyy");
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Valor = double.Parse(dt.Rows[i]["Valor"].ToString());
                item.Descricao = dt.Rows[i]["historico"].ToString();
                item.Conta_Id = (int)dt.Rows[i]["Conta_Id"];
                item.Nome_conta = dt.Rows[i]["nome"].ToString();
                item.Plano_Contas_Id = (int)dt.Rows[i]["Plano_Contas_Id"];
                item.DescricaoPlanoConta = dt.Rows[i]["plano_conta"].ToString();

                lista.Add(item);
            }

            return lista;
        }

        public TransacaoModel CarregaRegistro(int? id)
        {

            TransacaoModel item;

            string sql = "select t.id, t.Data, t.tipo, t.valor, t.descricao as historico, t.Conta_Id, c.nome, t.plano_contas_id, p.descricao as plano_conta "
                + " from transacao as t "
                + " inner join conta c on t.conta_id = c.id "
                + " inner join plano_contas as p on t.plano_contas_id = p.id"
                + $" where t.usuario_id = '{IdUsuarioLogado()}' and t.id = '{id}'";


            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);

            item = new TransacaoModel();
            item.Id = (int)dt.Rows[0]["Id"];
            item.Data = DateTime.Parse(dt.Rows[0]["Data"].ToString()).ToString("dd/MM/yyyy");
            item.Tipo = dt.Rows[0]["Tipo"].ToString();
            item.Valor = double.Parse(dt.Rows[0]["Valor"].ToString());
            item.Descricao = dt.Rows[0]["historico"].ToString();
            item.Conta_Id = (int)dt.Rows[0]["Conta_Id"];
            item.Nome_conta = dt.Rows[0]["nome"].ToString();
            item.Plano_Contas_Id = (int)dt.Rows[0]["Plano_Contas_Id"];
            item.DescricaoPlanoConta = dt.Rows[0]["plano_conta"].ToString();



            return item;
        }

        public void inserirTransacao()
        {
            DAL objDAL = new DAL();
            string sql;
            if (Id == 0)
            {
                sql = "insert into transacao (data, Tipo, descricao, valor, conta_id, plano_contas_id, usuario_id) " +
                    $" values ('{DateTime.Parse(Data).ToString("yyyyy/MM/dd")}','{Tipo}','{Descricao}', '{Valor}', '{Conta_Id}', '{Plano_Contas_Id}', '{IdUsuarioLogado()}' )";

            }
            else
            {
                sql = $"update transacao set Descricao = '{Descricao}', Tipo = '{Tipo}', valor = '{Valor}', conta_id = '{Conta_Id}', " +
                    $" plano_contas_id =  '{Plano_Contas_Id}' " +
                    $" where Usuario_id = '{IdUsuarioLogado()}' and id = '{Id}'";
            }
            objDAL.executaSQL(sql);
        }

        public void ExcluirTransacao(int Id)
        {
            new DAL().executaSQL("delete from transacao where Id = " + Id);
        }
    }

    public class Dashboard
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard() { }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public List<Dashboard> RetornaGrafico()
        {
            List<Dashboard> lista = new List<Dashboard>();
            Dashboard item;

            string sql = "select sum(t.valor) as Total, p.Descricao from transacao as t " +
                $"inner join plano_contas as p on t.Plano_Contas_id = p.id where t.tipo = 'D' and t.usuario_id = {IdUsuarioLogado()} " +
                "group by p.Descricao";

            DAL objDAL = new DAL();
            DataTable dt = new DataTable();
            dt = objDAL.retornaTabela(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["Total"].ToString());
                item.PlanoConta = dt.Rows[i]["Descricao"].ToString();
                lista.Add(item);
            }

            return lista;
        }
    }
}
