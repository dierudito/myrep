using DomainValidation.Interfaces.Specification;
using moreno.myrep.domain.Entities.Base;
using System.Linq.Expressions;

namespace moreno.myrep.domain.Specifications;

public class GenericSpecification<TEntity>(Expression<Func<TEntity, bool>> Expression) :
    ISpecification<TEntity> where TEntity : Entity
{
    public async Task<bool> IsSatisfiedByAsync(TEntity entity) =>
        Expression.Compile().Invoke(entity);
}
