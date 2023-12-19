using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;

namespace moreno.myrep.domain.Specifications.MesesTrabalho;

public class MesDeveSerUnicoParaAnoSpecification(IMesTrabalhoRepository MesTrabalhoRepository) :
    ISpecification<MesTrabalho>
{
    public async Task<bool> IsSatisfiedByAsync(MesTrabalho entity) =>
        (await MesTrabalhoRepository.Obter(mesTrabalho =>
        mesTrabalho.InicioPeriodo == entity.InicioPeriodo &&
        mesTrabalho.TerminoPeriodo == entity.TerminoPeriodo)) == null;
}
