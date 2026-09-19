using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using ControleCurriculo.Models;

namespace ControleCurriculo.DAO
{
    public class CurriculoDAO
    {
        private NpgsqlParameter[] CriaParametros(CurriculoViewModel c)
        {
            NpgsqlParameter[] p = new NpgsqlParameter[8];
            p[0] = new NpgsqlParameter("id", c.Id);
            p[1] = new NpgsqlParameter("cpf", c.Cpf);
            p[2] = new NpgsqlParameter("nome", c.Nome);
            p[3] = string.IsNullOrEmpty(c.Endereco) ? new NpgsqlParameter("endereco", DBNull.Value) : new NpgsqlParameter("endereco", c.Endereco);
            p[4] = string.IsNullOrEmpty(c.Telefone) ? new NpgsqlParameter("telefone", DBNull.Value) : new NpgsqlParameter("telefone", c.Telefone);
            p[5] = string.IsNullOrEmpty(c.Email) ? new NpgsqlParameter("email", DBNull.Value) : new NpgsqlParameter("email", c.Email);
            p[6] = c.PretensaoSalarial == null ? new NpgsqlParameter("pretensao_salarial", DBNull.Value) : new NpgsqlParameter("pretensao_salarial", c.PretensaoSalarial);
            p[7] = string.IsNullOrEmpty(c.CargoPretendido) ? new NpgsqlParameter("cargo_pretendido", DBNull.Value) : new NpgsqlParameter("cargo_pretendido", c.CargoPretendido);
            return p;
        }

        // Insere o currículo "pai" e retorna o Id gerado, pra usar nas tabelas filhas
        public int Inserir(CurriculoViewModel c)
        {
            string sql = @"insert into curriculo (cpf, nome, endereco, telefone, email, pretensao_salarial, cargo_pretendido)
                           values (@cpf, @nome, @endereco, @telefone, @email, @pretensao_salarial, @cargo_pretendido)
                           returning id";

            NpgsqlParameter[] p = new NpgsqlParameter[7];
            p[0] = new NpgsqlParameter("cpf", c.Cpf);
            p[1] = new NpgsqlParameter("nome", c.Nome);
            p[2] = string.IsNullOrEmpty(c.Endereco) ? new NpgsqlParameter("endereco", DBNull.Value) : new NpgsqlParameter("endereco", c.Endereco);
            p[3] = string.IsNullOrEmpty(c.Telefone) ? new NpgsqlParameter("telefone", DBNull.Value) : new NpgsqlParameter("telefone", c.Telefone);
            p[4] = string.IsNullOrEmpty(c.Email) ? new NpgsqlParameter("email", DBNull.Value) : new NpgsqlParameter("email", c.Email);
            p[5] = c.PretensaoSalarial == null ? new NpgsqlParameter("pretensao_salarial", DBNull.Value) : new NpgsqlParameter("pretensao_salarial", c.PretensaoSalarial);
            p[6] = string.IsNullOrEmpty(c.CargoPretendido) ? new NpgsqlParameter("cargo_pretendido", DBNull.Value) : new NpgsqlParameter("cargo_pretendido", c.CargoPretendido);

            return HelperDAO.ExecutaInsertRetornandoId(sql, p);
        }

        public void Alterar(CurriculoViewModel c)
        {
            string sql = @"update curriculo set
                cpf = @cpf, nome = @nome, endereco = @endereco, telefone = @telefone,
                email = @email, pretensao_salarial = @pretensao_salarial, cargo_pretendido = @cargo_pretendido
                where id = @id";

            HelperDAO.ExecutaSQL(sql, CriaParametros(c));
        }

        // Exclui o currículo — as tabelas filhas somem sozinhas por causa do ON DELETE CASCADE
        public void Excluir(int id)
        {
            string sql = "delete from curriculo where id = " + id;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public CurriculoViewModel Consulta(int id)
        {
            string sql = "select * from curriculo where id = " + id;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            if (tabela.Rows.Count == 0)
                return null;

            CurriculoViewModel c = MontaModel(tabela.Rows[0]);

            // Carrega as listas relacionadas
            c.Formacoes = FormacaoDAO.ListarPorCurriculo(id);
            c.Experiencias = ExperienciaDAO.ListarPorCurriculo(id);
            c.Idiomas = IdiomaDAO.ListarPorCurriculo(id);

            return c;
        }

        public static CurriculoViewModel MontaModel(DataRow r)
        {
            CurriculoViewModel c = new CurriculoViewModel();
            c.Id = Convert.ToInt32(r["id"]);
            c.Cpf = r["cpf"].ToString();
            c.Nome = r["nome"].ToString();
            c.Endereco = r["endereco"] != DBNull.Value ? r["endereco"].ToString() : null;
            c.Telefone = r["telefone"] != DBNull.Value ? r["telefone"].ToString() : null;
            c.Email = r["email"] != DBNull.Value ? r["email"].ToString() : null;
            if (r["pretensao_salarial"] != DBNull.Value)
                c.PretensaoSalarial = Convert.ToDouble(r["pretensao_salarial"]);
            c.CargoPretendido = r["cargo_pretendido"] != DBNull.Value ? r["cargo_pretendido"].ToString() : null;
            return c;
        }

        // Listagem simplificada — só CPF e Nome, como pede o enunciado (não carrega as listas filhas, por performance)
        public static List<CurriculoViewModel> Listagem()
        {
            List<CurriculoViewModel> lista = new List<CurriculoViewModel>();
            string sql = "select * from curriculo order by nome";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);

            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;

        }
    }
}
