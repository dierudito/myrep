using DomainValidation.Validation;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Specifications.DiasTrabalho;

namespace moreno.myrep.domain.Validations.DiasTrabalho;

public class DiaTrabalhoEstaAptoParaCadastroValidation : Validator<DiaTrabalho>
{
    public DiaTrabalhoEstaAptoParaCadastroValidation(IDiaTrabalhoRepository diaTrabalhoRepository)
    {
        var diaUnicio = new DiaTrabalhoDeveSerUnicoSpecification(diaTrabalhoRepository);

        Add("diaUnicio", new Rule<DiaTrabalho>(diaUnicio, "Ja existe esse dia cadastrado"));
    }
}
