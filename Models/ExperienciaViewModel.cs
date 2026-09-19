namespace ControleCurriculo.Models
{
    public class ExperienciaViewModel
    {
        public int Id { get; set; }
        public int CurriculoId { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public string Periodo { get; set; }
        public string Descricao { get; set; }
    }
}