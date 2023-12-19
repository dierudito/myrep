using moreno.myrep.domain.Entities.Base;
using moreno.myrep.domain.Enums;
using moreno.myrep.domain.Validations.Apontamentos;

namespace moreno.myrep.domain.Entities;

public class Apontamento : Entity
{
    public DateTime Hora { get; private set; }
    public EEtapa Etapa { get; private set; }
    public Guid DiaTrabalhoId { get; private set; }
    public DiaTrabalho DiaTrabalho { get; set; }

    public Apontamento(DateTime hora, EEtapa etapa, DiaTrabalho diaTrabalho)
    {
        DefinirHora(hora);
        Etapa = etapa;
        DiaTrabalho = diaTrabalho;
        DiaTrabalhoId = diaTrabalho.Id;
    }
    public Apontamento()
    {
        
    }

    private void DefinirHora(DateTime hora)
    {
        hora = hora
            .AddSeconds((-1) * hora.Second)
            .AddMicroseconds((-1)*hora.Microsecond)
            .AddMilliseconds((-1) * hora.Millisecond);
        Hora = Etapa switch
        {
            EEtapa.SaidaAlmoco or EEtapa.FimDoDia =>
                hora.AddMinutes(1).AddSeconds((-1) * hora.Second),
            _ => hora
        };
    }

    public override async Task<bool> EhValido()
    {
        ValidationResult = await new ApontamentoEstaConsistenteValidation().ValidateAsync(this);
        return ValidationResult.IsValid;
    }
}
