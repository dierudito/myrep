using DomainValidation.Validation;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Specifications;

namespace moreno.myrep.domain.Validations.Apontamentos;

public class ApontamentoEstaConsistenteValidation : Validator<Apontamento>
{
    public ApontamentoEstaConsistenteValidation()
    {
        var dataNaoPodeEstarNoPassado =
            new GenericSpecification<Apontamento>(apontamento =>
            apontamento.Hora >= DateTime.Now.AddMinutes(-1));

        Add("dataNaoPodeEstarNoPassado", new Rule<Apontamento>(dataNaoPodeEstarNoPassado, "A data nao pode estar no passado"));
    }
}