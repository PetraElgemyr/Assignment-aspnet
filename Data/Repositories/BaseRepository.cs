using Data.Contexts;
using Data.Entitites;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
{
    Task<RepositoryResult> AddAsync(TEntity entity);
    Task<RepositoryResult> DeleteAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortByColumn = null, Expression<Func<TEntity, bool>>? filterBy = null, int take = 0, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
{

    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }






    public virtual async Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortByColumn = null, Expression<Func<TEntity, bool>>? filterBy = null, int take = 0, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);

                //var memberExpression = include.Body as MemberExpression ??
                //          ((UnaryExpression)include.Body).Operand as MemberExpression;
                
                //if (memberExpression != null)
                //{
                //    var externalEntityType = memberExpression.Type;
                //    var mappingDictionary = new Dictionary<Type, Type> {
                //        { typeof(UserEntity), typeof(User) },
                //        { typeof(ClientEntity), typeof(Client) },
                //        { typeof(StatusEntity), typeof(Status) }
                //    };
                    
                //    var destinationType = mappingDictionary.TryGetValue(externalEntityType, out var dtoType) ? dtoType : null;
                //    if (destinationType != null)
                //    {
                //        var externalEntities = query.Select(include).ToList(); // Hämta relaterade entiteter
                //        var mappedMethod = typeof(MappingExtensions)
                //            .GetMethod(nameof(MappingExtensions.MapTo))
                //            .MakeGenericMethod(destinationType); // Anropa MapTo<UserModel>() dynamiskt
                //        var mappedEntities = externalEntities
                //            .Select(e => mappedMethod.Invoke(null, new[] { e }))
                //            .ToList();
                //    }
                //}
               
            }
        }

        if (sortByColumn != null)
            query = orderByDescending
                ? query.OrderByDescending(sortByColumn)
                : query.OrderBy(sortByColumn);

        if (take > 0)
        {
            query = query.Take(take);
        }

        var entities = await query.ToListAsync();

        // TODO fråga hans om att mappa alla externa entiteter för sig?? innan mappning av projects
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return new RepositoryResult<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = result };
    }


    public virtual async Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (findBy == null)
            return new RepositoryResult<TModel> { Succeeded = false, StatusCode = 400, Error = "Expression not defined." };

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(findBy);

        if (entity == null)
            return new RepositoryResult<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found." };


        var result = entity.MapTo<TModel>();

        return new RepositoryResult<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }


    public virtual async Task<RepositoryResult> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        if (findBy == null)
            return new RepositoryResult { Succeeded = false, StatusCode = 400, Error = "Invalid expression" };

        if (!await _table.AnyAsync(findBy))
            return new RepositoryResult { Succeeded = false, StatusCode = 404, Error = "Entity not found." };

        return new RepositoryResult { Succeeded = true, StatusCode = 200, Error = "Entity exists." };
    }


    public virtual async Task<RepositoryResult> AddAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
                return new RepositoryResult { Succeeded = false, StatusCode = 400, Error = "Invalid properties" };

            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult> UpdateAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
                return new RepositoryResult { Succeeded = false, StatusCode = 400, Error = "Invalid properties" };

            if (!await _table.ContainsAsync(entity))
                return new RepositoryResult { Succeeded = false, StatusCode = 404, Error = "Entity not found." };

            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult> DeleteAsync(Expression<Func<TEntity, bool>> findBy)
    {
        try
        {
            if (findBy == null)
                return new RepositoryResult { Succeeded = false, StatusCode = 400, Error = "Invalid expression" };

            var entity = await _table.FirstOrDefaultAsync(findBy);
            if (entity == null)
                return new RepositoryResult { Succeeded = false, StatusCode = 404, Error = "Entity not found." };

            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
}
