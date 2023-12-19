using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;

namespace moreno.myrep.domain.Specifications.DiasTrabalho;

public class DiaTrabalhoDeveSerUnicoSpecification(IDiaTrabalhoRepository diaTrabalhoRepository)
    : ISpecification<DiaTrabalho>
{
    public async Task<bool> IsSatisfiedByAsync(DiaTrabalho entity)
    {
        var dia = await diaTrabalhoRepository
            .ObterUnico(diaTrabalho => diaTrabalho.DataDiaTrabalho == entity.DataDiaTrabalho);

        return dia == null;
    }
}
