namespace ControleCurriculo.Models
{
    public class FormacaoViewModel
    {
        public int Id { get; set; }
        public int CurriculoId { get; set; }
        public string Curso { get; set; }
        public string Instituicao { get; set; }
        public int? AnoConclusao { get; set; }
    }
}