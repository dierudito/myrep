using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Services.Base;

namespace moreno.myrep.domain.Interfaces.Services;

public interface IMesTrabalhoService : IBaseService<MesTrabalho>
{
    Task<MesTrabalho> CriarApontamentoParaMesAsync(MesTrabalho mesTrabalho);
}