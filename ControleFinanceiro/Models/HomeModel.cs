using ControleFinanceiro.Util;
using System.Data;


namespace ControleFinanceiro.Models
{
    public class HomeModel
    {
        public string lerUsuario()
        {
            DAL ojbDAL = new DAL();
            DataTable dt = ojbDAL.retornaTabela("select Nome from usuario");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Nome"].ToString();
                }
            }

            return "Nome não econtrado ";

        }
    }
}
