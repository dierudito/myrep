using moreno.myrep.api.Models.Base;

namespace moreno.myrep.api.Models
{
    public class Apontamento : Entity
    {
        public DateTime Hora { get; set; }
        public EEtapa Etapa { get; set; }
        public Guid DiaTrabalhoId { get; set; }
        public DiaTrabalho DiaTrabalho { get; set; }
    }
}
