using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories.Base;

namespace moreno.myrep.domain.Interfaces.Repositories;

public interface IApontamentoRepository : IBaseRepository<Apontamento>
{
    Task<Apontamento?> ObterUltimoApontamentoDoDiaAsync(DiaTrabalho diaTrabalho);
}