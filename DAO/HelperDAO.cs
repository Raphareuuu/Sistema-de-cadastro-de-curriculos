using Npgsql;
using System;
using System.Data;

namespace ControleCurriculo.DAO
{
    public static class HelperDAO
    {
        public static void ExecutaSQL(string sql, NpgsqlParameter[] p)
        {
            using (NpgsqlConnection conexao = ConexaoBD.GetConexao())
            {
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                if (p != null)
                    comando.Parameters.AddRange(p);
                comando.ExecuteNonQuery();
            }
        }
        public static DataTable ExecutaSelect(string sql, NpgsqlParameter[] parametros)
        {
            using (NpgsqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conexao))
                {
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);

                    DataTable tabelaTemp = new DataTable();
                    adapter.Fill(tabelaTemp);
                    conexao.Close();
                    return tabelaTemp;
                }
            }
        }

        // Executa um INSERT que tem "RETURNING id" no SQL e devolve o Id gerado
        public static int ExecutaInsertRetornandoId(string sql, NpgsqlParameter[] p)
        {
            using (NpgsqlConnection conexao = ConexaoBD.GetConexao())
            {
                NpgsqlCommand comando = new NpgsqlCommand(sql, conexao);
                if (p != null)
                    comando.Parameters.AddRange(p);

                object resultado = comando.ExecuteScalar();
                return Convert.ToInt32(resultado);
            }
        }
    }
}