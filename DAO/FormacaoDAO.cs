using ControleCurriculo.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ControleCurriculo.DAO
{
    public class FormacaoDAO
    {
        private NpgsqlParameter[] CriaParametros(FormacaoViewModel f)
        {
            NpgsqlParameter[] p = new NpgsqlParameter[5];
            p[0] = new NpgsqlParameter("id", f.Id);
            p[1] = new NpgsqlParameter("curriculo_id", f.CurriculoId);
            p[2] = new NpgsqlParameter("curso", f.Curso);
            p[3] = new NpgsqlParameter("instituicao", f.Instituicao);
            p[4] = f.AnoConclusao == null
                ? new NpgsqlParameter("ano_conclusao", DBNull.Value)
                : new NpgsqlParameter("ano_conclusao", f.AnoConclusao);
            return p;
        }

        public void Inserir(FormacaoViewModel f)
        {
            string sql = @"insert into formacao (curriculo_id, curso, instituicao, ano_conclusao)
                           values (@curriculo_id, @curso, @instituicao, @ano_conclusao)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(f));
        }

        public void ExcluirPorCurriculo(int curriculoId)
        {
            string sql = "delete from formacao where curriculo_id = " + curriculoId;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public static FormacaoViewModel MontaModel(DataRow r)
        {
            FormacaoViewModel f = new FormacaoViewModel();
            f.Id = Convert.ToInt32(r["id"]);
            f.CurriculoId = Convert.ToInt32(r["curriculo_id"]);
            f.Curso = r["curso"].ToString();
            f.Instituicao = r["instituicao"].ToString();
            if (r["ano_conclusao"] != DBNull.Value)
                f.AnoConclusao = Convert.ToInt32(r["ano_conclusao"]);
            return f;
        }

        public static List<FormacaoViewModel> ListarPorCurriculo(int curriculoId)
        {
            List<FormacaoViewModel> lista = new List<FormacaoViewModel>();
            string sql = "select * from formacao where curriculo_id = " + curriculoId + " order by id";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}