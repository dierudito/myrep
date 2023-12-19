using moreno.myrep.domain.Dtos;
using moreno.myrep.domain.Entities.Base;
using System.Linq.Expressions;

namespace moreno.myrep.domain.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> ObterTodosAsync();
    Task<TEntity> ObterPorId(object id);
    Task<(IEnumerable<TEntity> items, int qtd)> Obter(Expression<Func<TEntity, bool>>? filtro = null,
        PaginacaoEntradaDto? paginacao = null,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> ObterUnico(Expression<Func<TEntity, bool>> expression);

    Task<IEnumerable<TEntity>> Obter(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> ObterParaEscrita(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> Adicionar(TEntity entidade);
    Task<TEntity> Atualizar(TEntity entidade);
    Task<TEntity> Atualizar(TEntity updated, int key);
    Task Deletar(long id);
    Task Deletar(TEntity entidade);
    Task DeletarRange(Expression<Func<TEntity, bool>>? filtro = null);
    Task<int> CommitAsync();
}
