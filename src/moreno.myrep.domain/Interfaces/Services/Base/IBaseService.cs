using moreno.myrep.domain.Entities.Base;
using System.Linq.Expressions;

namespace moreno.myrep.domain.Interfaces.Services.Base;

public interface IBaseService<TEntity> where TEntity : Entity
{
    Task<TEntity> Adicionar(TEntity entidade);
    Task<TEntity> Atualizar(TEntity entidade);
    Task<TEntity> Atualizar(TEntity updated, int key);
    Task Deletar(long id);
    Task Deletar(TEntity entidade);
    Task DeletarRange(Expression<Func<TEntity, bool>>? filtro = null);
}
