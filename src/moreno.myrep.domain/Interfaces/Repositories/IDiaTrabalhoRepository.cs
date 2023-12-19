using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories.Base;

namespace moreno.myrep.domain.Interfaces.Repositories;

public interface IDiaTrabalhoRepository : IBaseRepository<DiaTrabalho>
{
    Task<DiaTrabalho> ObterPorDataAsync(DateTime diaTrabalho);
}
