using DomainValidation.Validation;

namespace moreno.myrep.domain.Entities.Base;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        ValidationResult = new();
    }

    public void AdicionarErroValidacao(string erro, string mensagem)
    {
        ValidationResult.Add(new ValidationError(erro, mensagem));
    }

    public void AdicionarErrosValidacao(ValidationResult validationResult)
    {
        ValidationResult.Add(validationResult);
    }

    public void ZerarListaErros()
    {
        ValidationResult = new();
    }

    public abstract Task<bool> EhValido();
}