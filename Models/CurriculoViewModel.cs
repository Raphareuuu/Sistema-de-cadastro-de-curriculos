using System.Collections.Generic;

namespace ControleCurriculo.Models
{
    public class CurriculoViewModel
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public double? PretensaoSalarial { get; set; }
        public string CargoPretendido { get; set; }

        public List<FormacaoViewModel> Formacoes { get; set; } = new List<FormacaoViewModel>();
        public List<ExperienciaViewModel> Experiencias { get; set; } = new List<ExperienciaViewModel>();
        public List<IdiomaViewModel> Idiomas { get; set; } = new List<IdiomaViewModel>();
    }
}
