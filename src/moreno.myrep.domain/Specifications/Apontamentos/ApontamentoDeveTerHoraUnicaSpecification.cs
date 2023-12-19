using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;

namespace moreno.myrep.domain.Specifications.Apontamentos;

public class ApontamentoDeveTerEtapaUnicaParaDiaSpecification (IApontamentoRepository apontamentoRepository)
    : ISpecification<Apontamento>
{
    public async Task<bool> IsSatisfiedByAsync(Apontamento entity)
    {
        var apontamentos = await apontamentoRepository
            .Obter(apontamento => apontamento.DiaTrabalhoId == entity.DiaTrabalhoId &&
                                  apontamento.Etapa == entity.Etapa);

        return !apontamentos.Any();
    }
}