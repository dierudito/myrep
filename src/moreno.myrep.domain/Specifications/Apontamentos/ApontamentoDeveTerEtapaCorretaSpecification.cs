using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;

namespace moreno.myrep.domain.Specifications.Apontamentos;

public class ApontamentoDeveTerEtapaCorretaSpecification(IApontamentoRepository apontamentoRepository)
    : ISpecification<Apontamento>
{
    public async Task<bool> IsSatisfiedByAsync(Apontamento entity)
    {
        var apontamentosDoDia = await apontamentoRepository
            .Obter(apontamento => apontamento.DiaTrabalhoId == entity.DiaTrabalhoId);

        if (!apontamentosDoDia.Any()) return true;

        var ultimaEtapaCadastrada = apontamentosDoDia.Max(a => a.Etapa);
        if (ultimaEtapaCadastrada == Enums.EEtapa.FimDoDia) return false;

        return (int)entity.Etapa == ((int)ultimaEtapaCadastrada)+1;
    }
}
