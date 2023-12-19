using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;

namespace moreno.myrep.domain.Specifications.MesesTrabalho;

public class MesDeveTerInicioPeriodoValidoSpecification(int dia) :
    ISpecification<MesTrabalho>
{
    public async Task<bool> IsSatisfiedByAsync(MesTrabalho entity) =>
        entity.InicioPeriodo.Day == dia;
}
