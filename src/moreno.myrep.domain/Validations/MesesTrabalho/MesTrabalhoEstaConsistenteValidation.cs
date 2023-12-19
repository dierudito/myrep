using DomainValidation.Validation;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Specifications.MesesTrabalho;

namespace moreno.myrep.domain.Validations.MesesTrabalho;

public class MesTrabalhoEstaConsistenteValidation : Validator<MesTrabalho>
{
    public MesTrabalhoEstaConsistenteValidation()
    {
        var diaInicioMes = 1;
        var mesDeveTerInicioPeriodoValido = new MesDeveTerInicioPeriodoValidoSpecification(diaInicioMes);
        var mesDeveTerTerminoPeriodoValido = new MesDeveTerTerminoPeriodoValidoSpecification();
        var mesDeveTerPeriodoValido = new MesDeveTerPeriodoValidoSpecification();

        Add("mesDeveTerInicioPeriodoValido", new Rule<MesTrabalho>(mesDeveTerInicioPeriodoValido, $"O mês deve iniciar no dia {diaInicioMes}"));
        Add("mesDeveTerTerminoPeriodoValido", new Rule<MesTrabalho>(mesDeveTerTerminoPeriodoValido, $"O mês deve terminar no último dia do Mês"));
        Add("mesDeveTerPeriodoValido", new Rule<MesTrabalho>(mesDeveTerPeriodoValido, $"Período inválido. O período deve iniciar e terminar no mesmo mês"));

    }
}
