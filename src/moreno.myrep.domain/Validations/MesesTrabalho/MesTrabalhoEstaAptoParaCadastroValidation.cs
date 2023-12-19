using DomainValidation.Validation;
using moreno.myrep.domain.Entities;
using moreno.myrep.domain.Interfaces.Repositories;
using moreno.myrep.domain.Specifications.MesesTrabalho;

namespace moreno.myrep.domain.Validations.MesesTrabalho;

public class MesTrabalhoEstaAptoParaCadastroValidation : Validator<MesTrabalho>
{
    public MesTrabalhoEstaAptoParaCadastroValidation(IMesTrabalhoRepository mesTrabalhoRepository)
    {
        var mesUnicoParaAno = new MesDeveSerUnicoParaAnoSpecification(mesTrabalhoRepository);

        Add("mesUnicoParaAno", new Rule<MesTrabalho>(mesUnicoParaAno, "Já existe esse mês cadastrado"));
    }
}