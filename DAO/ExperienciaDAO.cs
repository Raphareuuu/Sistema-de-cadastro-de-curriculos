using ControleCurriculo.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ControleCurriculo.DAO
{
    public class ExperienciaDAO
    {
        public void Inserir(ExperienciaViewModel e)
        {
            string sql = @"insert into experiencia (curriculo_id, empresa, cargo, periodo, descricao)
                           values (@curriculo_id, @empresa, @cargo, @periodo, @descricao)";

            NpgsqlParameter[] p = new NpgsqlParameter[5];
            p[0] = new NpgsqlParameter("curriculo_id", e.CurriculoId);
            p[1] = string.IsNullOrEmpty(e.Empresa) ? new NpgsqlParameter("empresa", DBNull.Value) : new NpgsqlParameter("empresa", e.Empresa);
            p[2] = string.IsNullOrEmpty(e.Cargo) ? new NpgsqlParameter("cargo", DBNull.Value) : new NpgsqlParameter("cargo", e.Cargo);
            p[3] = string.IsNullOrEmpty(e.Periodo) ? new NpgsqlParameter("periodo", DBNull.Value) : new NpgsqlParameter("periodo", e.Periodo);
            p[4] = string.IsNullOrEmpty(e.Descricao) ? new NpgsqlParameter("descricao", DBNull.Value) : new NpgsqlParameter("descricao", e.Descricao);

            HelperDAO.ExecutaSQL(sql, p);
        }

        public void ExcluirPorCurriculo(int curriculoId)
        {
            string sql = "delete from experiencia where curriculo_id = " + curriculoId;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public static ExperienciaViewModel MontaModel(DataRow r)
        {
            ExperienciaViewModel e = new ExperienciaViewModel();
            e.Id = Convert.ToInt32(r["id"]);
            e.CurriculoId = Convert.ToInt32(r["curriculo_id"]);
            e.Empresa = r["empresa"] != DBNull.Value ? r["empresa"].ToString() : null;
            e.Cargo = r["cargo"] != DBNull.Value ? r["cargo"].ToString() : null;
            e.Periodo = r["periodo"] != DBNull.Value ? r["periodo"].ToString() : null;
            e.Descricao = r["descricao"] != DBNull.Value ? r["descricao"].ToString() : null;
            return e;
        }

        public static List<ExperienciaViewModel> ListarPorCurriculo(int curriculoId)
        {
            List<ExperienciaViewModel> lista = new List<ExperienciaViewModel>();
            string sql = "select * from experiencia where curriculo_id = " + curriculoId + " order by id";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}