using moreno.myrep.api.Models.Base;

namespace moreno.myrep.api.Models
{
    public class MesTrabalho : Entity
    {
        public string Cliente { get; set; }
        public string PrestadorServico { get; set; }
        public string Consultoria { get; set; }
        public DateTime InicioPeriodo { get; set; }
        public DateTime TerminoPeriodo { get; set; }
    }
}
