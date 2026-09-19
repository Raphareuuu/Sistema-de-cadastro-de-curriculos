using ControleCurriculo.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ControleCurriculo.DAO
{
    public class IdiomaDAO
    {
        public void Inserir(IdiomaViewModel i)
        {
            string sql = @"insert into idioma (curriculo_id, nome, nivel)
                           values (@curriculo_id, @nome, @nivel)";

            NpgsqlParameter[] p = new NpgsqlParameter[3];
            p[0] = new NpgsqlParameter("curriculo_id", i.CurriculoId);
            p[1] = string.IsNullOrEmpty(i.Nome) ? new NpgsqlParameter("nome", DBNull.Value) : new NpgsqlParameter("nome", i.Nome);
            p[2] = string.IsNullOrEmpty(i.Nivel) ? new NpgsqlParameter("nivel", DBNull.Value) : new NpgsqlParameter("nivel", i.Nivel);

            HelperDAO.ExecutaSQL(sql, p);
        }

        public void ExcluirPorCurriculo(int curriculoId)
        {
            string sql = "delete from idioma where curriculo_id = " + curriculoId;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public static IdiomaViewModel MontaModel(DataRow r)
        {
            IdiomaViewModel i = new IdiomaViewModel();
            i.Id = Convert.ToInt32(r["id"]);
            i.CurriculoId = Convert.ToInt32(r["curriculo_id"]);
            i.Nome = r["nome"] != DBNull.Value ? r["nome"].ToString() : null;
            i.Nivel = r["nivel"] != DBNull.Value ? r["nivel"].ToString() : null;
            return i;
        }

        public static List<IdiomaViewModel> ListarPorCurriculo(int curriculoId)
        {
            List<IdiomaViewModel> lista = new List<IdiomaViewModel>();
            string sql = "select * from idioma where curriculo_id = " + curriculoId + " order by id";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}