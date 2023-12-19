using moreno.myrep.domain.Entities.Base;
using moreno.myrep.domain.Interfaces.Repositories.Base;
using moreno.myrep.domain.Interfaces.Services.Base;
using System.Linq.Expressions;

namespace moreno.myrep.domain.Services.Base;

public abstract class BaseService<TEntity>(IBaseRepository<TEntity> Repository) : IBaseService<TEntity>
    where TEntity : Entity
{
    public virtual async Task<TEntity> Adicionar(TEntity entidade) =>
        await Repository.Adicionar(entidade);

    public async virtual Task<TEntity> Atualizar(TEntity entidade) =>
        await Repository.Atualizar(entidade);

    public virtual async Task<TEntity> Atualizar(TEntity updated, int key) =>
        await Repository.Atualizar(updated, key);

    public async Task Deletar(long id) =>
        await Repository.Deletar(id);

    public async Task Deletar(TEntity entidade) =>
        await Repository.Deletar(entidade);

    public async Task DeletarRange(Expression<Func<TEntity, bool>>? filtro = null) =>
        await Repository.DeletarRange(filtro);
}
