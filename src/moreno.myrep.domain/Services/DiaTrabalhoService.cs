using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Interfaces.Services;
using moreno.myrep.domain.Services.Base;

namespace moreno.myrep.domain.Services;

public class DiaTrabalhoService(IDiaTrabalhoRepository DiaTrabalhoRepository) :
    BaseService<DiaTrabalho>(DiaTrabalhoRepository), IDiaTrabalhoService
{
}

