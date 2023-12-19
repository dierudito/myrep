using DomainValidation.Validation;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Specifications.Apontamentos;

namespace moreno.myrep.domain.Validations.Apontamentos;

public class ApontamentoEstaAptoParaCadastroValidation : Validator<Apontamento>
{
    public ApontamentoEstaAptoParaCadastroValidation(IApontamentoRepository apontamentoRepository)
    {
        var deveTerEtapaCorreta = new ApontamentoDeveTerEtapaCorretaSpecification(apontamentoRepository);

        Add("deveTerEtapaCorreta", new Rule<Apontamento>(deveTerEtapaCorreta, "Etapa incorreta"));
    }
}