using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;

namespace moreno.myrep.domain.Specifications.MesesTrabalho;

public class MesDeveTerTerminoPeriodoValidoSpecification() :
    ISpecification<MesTrabalho>
{
    public async Task<bool> IsSatisfiedByAsync(MesTrabalho entity)
    {
        var data = entity.TerminoPeriodo;
        var dataCorreta = new DateTime(data.Year, data.Month, data.Day).AddMonths(1).AddDays(-1);

        return data == dataCorreta;
    }
}
