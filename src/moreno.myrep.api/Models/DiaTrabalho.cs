using moreno.myrep.api.Models.Base;

namespace moreno.myrep.api.Models
{
    public class DiaTrabalho : Entity
    {
        public DateTime DataDiaTrabalho { get; set; }
        public string? Descricao { get; set; }
        public string? Cliente { get; set; }
        public Guid MesTrabalhoId { get; set; }
        public MesTrabalho MesTrabalho { get; set; }
    }
}
