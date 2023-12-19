using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Services.Base;

namespace moreno.myrep.domain.Interfaces.Services;

public interface IApontamentoService : IBaseService<Apontamento>
{
    Task<Apontamento> BaterPontoAsync(string? descricao, string? cliente);
}