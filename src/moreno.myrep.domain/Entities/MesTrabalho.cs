using moreno.myrep.domain.Entities.Base;
using moreno.myrep.domain.Validations.MesesTrabalho;

namespace moreno.myrep.domain.Entities;

public class MesTrabalho(string cliente, string prestadorServico, string consultoria,
    DateTime inicioPeriodo, DateTime terminoPeriodo) : Entity
{
    public string Cliente { get; private set; } = cliente;
    public string PrestadorServico { get; private set; } = prestadorServico;
    public string Consultoria { get; private set; } = consultoria;
    public DateTime InicioPeriodo { get; private set; } = inicioPeriodo;
    public DateTime TerminoPeriodo { get; private set; } = terminoPeriodo;
    public virtual IList<DiaTrabalho> DiasTrabalho { get; private set; } = [];

    public MesTrabalho() : this("", "", "", DateTime.Now, DateTime.Now)
    {
        
    }

    public override async Task<bool> EhValido()
    {
        ValidationResult = await new MesTrabalhoEstaConsistenteValidation().ValidateAsync(this);
        return ValidationResult.IsValid;
    }

    public async Task AdicionarDiaTrabalho(DiaTrabalho diaTrabalho)
    {
        if (!await diaTrabalho.EhValido())
        {
            AdicionarErrosValidacao(diaTrabalho.ValidationResult);
            return;
        }
        DiasTrabalho.Add(diaTrabalho);
    }
}
