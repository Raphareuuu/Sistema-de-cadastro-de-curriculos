using Npgsql;

namespace ControleCurriculo.DAO
{
    public static class ConexaoBD
    {
        public static NpgsqlConnection GetConexao()
        {
            string strCon = "Host=localhost;Port=5432;Database=controlecurriculo;Username=postgres;Password=123456";
            NpgsqlConnection conexao = new NpgsqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
