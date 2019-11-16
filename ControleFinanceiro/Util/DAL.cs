using MySql.Data.MySqlClient;
using System.Data;

namespace ControleFinanceiro.Util
{
    public class DAL
    {
        private static string servidor = "localhost";
        private static string baseDados = "financeiro";
        private static string usuario = "root";
        private static string senha = "1234";
        private static string stringConexao = $"Server={servidor}; Database={baseDados}; Uid={usuario}; Password={senha}; SslMode=none";
        private static MySqlConnection conexao;

        public DAL()
        {
            conexao = new MySqlConnection(stringConexao);
            conexao.Open();
        }

        public object MysqlAdapter { get; private set; }

        public DataTable retornaTabela(string sql)
        {
            DataTable tabela = new DataTable();
            MySqlCommand comando = new MySqlCommand(sql, conexao);
            MySqlDataAdapter da = new MySqlDataAdapter(comando);
            da.Fill(tabela);

            return tabela;
            
        }

        public void executaSQL(string sql)
        {
            MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.ExecuteNonQuery();

        }
    }
}
