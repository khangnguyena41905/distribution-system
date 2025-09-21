using System.Linq.Expressions;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using IDENTITY.DOMAIN.Abstractions.Repositories;

namespace IDENTITY.PERSISTENCE.Repositories;

public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>, IDisposable
    where TEntity : class
{
    protected readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
        => _context = context;

    public void Dispose() => _context?.Dispose();

    public async Task<TEntity?> FindByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = ApplyIncludes(_context.Set<TEntity>(), includeProperties);
        return await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id")!.Equals(id));
    }

    public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = ApplyIncludes(_context.Set<TEntity>(), includeProperties);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = ApplyIncludes(_context.Set<TEntity>(), includeProperties);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<PagedResult<TEntity>> FindAllPagedAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = ApplyIncludes(_context.Set<TEntity>(), includeProperties);

        if (predicate is not null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return PagedResult<TEntity>.Create(items, totalCount, pageIndex, pageSize);
    }
    
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var entry = await _context.Set<TEntity>().AddAsync(entity);
        return entry.Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Update(entity);
        return Task.FromResult(entry.Entity);
    }

    public Task RemoveAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveMultipleAsync(List<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task VirtualRemoveAsync(TEntity entity)
    {
        if (entity is AuditEntity<TKey> domainEntity)
        {
            domainEntity.IsDeleted = true;
            _context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} is not a domain entity");
    }
    
    public Task VirtualRemoveMultipleAsync(List<TEntity> entities)
    {
        if (entities == null || !entities.Any())
        {
            throw new ArgumentNullException(nameof(entities), "Entity list is null or empty");
        }

        foreach (var entity in entities)
        {
            if (entity is AuditEntity<TKey> domainEntity)
            {
                domainEntity.IsDeleted = true;
                _context.Set<TEntity>().Update(entity);
            }
            else
            {
                throw new ArgumentException("One or more entities are not of type AuditEntity", nameof(entities));
            }
        }

        return Task.CompletedTask;
    }


    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        if (includeProperties != null)
        {
            foreach (var include in includeProperties)
                query = query.Include(include);
        }

        return query;
    }
}