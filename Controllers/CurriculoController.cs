using ControleCurriculo.DAO;
using ControleCurriculo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleCurriculo.Controllers
{
    public class CurriculoController : Controller
    {
        private void GarantirSlots(CurriculoViewModel c)
        {
            while (c.Formacoes.Count < 5)
                c.Formacoes.Add(new FormacaoViewModel());

            while (c.Experiencias.Count < 3)
                c.Experiencias.Add(new ExperienciaViewModel());

            while (c.Idiomas.Count < 3)
                c.Idiomas.Add(new IdiomaViewModel());
        }

        // GET: /curriculo (Listagem)
        public IActionResult Index()
        {
            List<CurriculoViewModel> lista = CurriculoDAO.Listagem();
            return View(lista);
        }

        // GET: /curriculo/create (formulário vazio)
        public IActionResult Create()
        {
            CurriculoViewModel c = new CurriculoViewModel();
            GarantirSlots(c);
            return View("Form", c);
        }

        // GET: /curriculo/edit?id=... (formulário preenchido)
        public IActionResult Edit(int id)
        {
            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                CurriculoViewModel c = dao.Consulta(id);

                if (c == null)
                    return RedirectToAction("Index");

                GarantirSlots(c);
                return View("Form", c);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel { RequestId = erro.ToString() });
            }
        }

        // POST: /curriculo/salvar
        [HttpPost]
        public IActionResult Salvar(CurriculoViewModel curriculo)
        {
            // Validação: pelo menos a primeira formação é obrigatória
            var primeiraFormacao = curriculo.Formacoes.FirstOrDefault();

            if (primeiraFormacao == null ||
                string.IsNullOrWhiteSpace(primeiraFormacao.Curso) ||
                string.IsNullOrWhiteSpace(primeiraFormacao.Instituicao))
            {
                ModelState.AddModelError("", "É obrigatório informar ao menos a primeira formação acadêmica (curso e instituição).");
                return View("Form", curriculo);
            }

            try
            {
                CurriculoDAO curriculoDAO = new CurriculoDAO();
                FormacaoDAO formacaoDAO = new FormacaoDAO();
                ExperienciaDAO experienciaDAO = new ExperienciaDAO();
                IdiomaDAO idiomaDAO = new IdiomaDAO();

                int curriculoId;

                if (curriculo.Id == 0)
                {
                    curriculoId = curriculoDAO.Inserir(curriculo);
                }
                else
                {
                    curriculoDAO.Alterar(curriculo);
                    curriculoId = curriculo.Id;

                    formacaoDAO.ExcluirPorCurriculo(curriculoId);
                    experienciaDAO.ExcluirPorCurriculo(curriculoId);
                    idiomaDAO.ExcluirPorCurriculo(curriculoId);
                }

                foreach (var formacao in curriculo.Formacoes)
                {
                    if (!string.IsNullOrEmpty(formacao.Curso) && !string.IsNullOrEmpty(formacao.Instituicao))
                    {
                        formacao.CurriculoId = curriculoId;
                        formacaoDAO.Inserir(formacao);
                    }
                }

                foreach (var experiencia in curriculo.Experiencias)
                {
                    if (!string.IsNullOrEmpty(experiencia.Empresa))
                    {
                        experiencia.CurriculoId = curriculoId;
                        experienciaDAO.Inserir(experiencia);
                    }
                }

                foreach (var idioma in curriculo.Idiomas)
                {
                    if (!string.IsNullOrEmpty(idioma.Nome))
                    {
                        idioma.CurriculoId = curriculoId;
                        idiomaDAO.Inserir(idioma);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel { RequestId = erro.ToString() });
            }
        }

        // GET: /curriculo/delete?id=...
        public IActionResult Delete(int id)
        {
            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                dao.Excluir(id); // as tabelas filhas somem sozinhas (ON DELETE CASCADE)
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel { RequestId = erro.ToString() });
            }
        }

        // GET: /curriculo/exibir?id=... (visualização formatada com CSS)
        public IActionResult Exibir(int id)
        {
            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                CurriculoViewModel c = dao.Consulta(id);

                if (c == null)
                    return RedirectToAction("Index");

                return View(c);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel { RequestId = erro.ToString() });
            }
        }
    }
}