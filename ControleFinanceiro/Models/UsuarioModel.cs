using ControleFinanceiro.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ControleFinanceiro.Models
{
    public class UsuarioModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "O Nome deve ser preenchido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A Senha deve ser preenchida")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O Email deve ser preenchido")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "o Email informado é invalido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A Data de nascimento deve ser preenchida")]
        public string dataNascimento { get; set; }

        public bool validarLogin()
        {
            string sql = $"select Id, Nome, Email, Senha from usuario where Email = '{Email}' and Senha = '{Senha}'";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.retornaTabela(sql);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Id = int.Parse(dt.Rows[0]["Id"].ToString());
                    Nome = dt.Rows[0]["Nome"].ToString();
                    return true;
                }
            }

            return false;

        }

        public void RegistrarUsuario()
        {
            string Data = DateTime.Parse(dataNascimento).ToString("yyyy/MM/dd");
            string sql = $"insert into usuario (Nome, Senha, Email, Data_Nascimento) values ('{Nome}','{Senha}', '{Email}', '{Data}')";
            DAL objDAL = new DAL();
            objDAL.executaSQL(sql);    
        }
    }
}
