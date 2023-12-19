using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;

namespace moreno.myrep.domain.Specifications.MesesTrabalho;

public class MesDeveTerPeriodoValidoSpecification :
    ISpecification<MesTrabalho>
{
    public async Task<bool> IsSatisfiedByAsync(MesTrabalho entity) =>
        entity.InicioPeriodo.Month == entity.TerminoPeriodo.Month &&
        entity.InicioPeriodo.Year == entity.TerminoPeriodo.Year;
}