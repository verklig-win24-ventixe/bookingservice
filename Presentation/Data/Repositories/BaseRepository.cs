using System.Linq.Expressions;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Interfaces;

namespace Presentation.Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
  protected readonly DataContext _context = context;
  protected readonly DbSet<TEntity> _db = context.Set<TEntity>();

  public virtual async Task<RepositoryResult> AddAsync(TEntity entity)
  {
    try
    {
      _db.Add(entity);
      await _context.SaveChangesAsync();

      return new RepositoryResult { Success = true };
    }
    catch (Exception ex)
    {
      return new RepositoryResult { Success = false, Error = ex.Message };
    }
  }

  public virtual async Task<RepositoryResult> UpdateAsync(TEntity entity)
  {
    try
    {
      _db.Update(entity);
      await _context.SaveChangesAsync();

      return new RepositoryResult { Success = true };
    }
    catch (Exception ex)
    {
      return new RepositoryResult { Success = false, Error = ex.Message };
    }
  }

  public virtual async Task<RepositoryResult> DeleteAsync(TEntity entity)
  {
    try
    {
      _db.Remove(entity);
      await _context.SaveChangesAsync();

      return new RepositoryResult { Success = true };
    }
    catch (Exception ex)
    {
      return new RepositoryResult { Success = false, Error = ex.Message };
    }
  }

  public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
  {
    try
    {
      var entities = await _db.ToListAsync();

      return new RepositoryResult<IEnumerable<TEntity>> { Success = true, Result = entities };
    }
    catch (Exception ex)
    {
      return new RepositoryResult<IEnumerable<TEntity>> { Success = false, Error = ex.Message };
    }
  }

  public virtual async Task<RepositoryResult<TEntity?>> GetAsync(Expression<Func<TEntity, bool>> expression)
  {
    try
    {
      var entity = await _db.FirstOrDefaultAsync(expression) ?? throw new Exception("Entity was not found.");

      return new RepositoryResult<TEntity?> { Success = true, Result = entity };
    }
    catch (Exception ex)
    {
      return new RepositoryResult<TEntity?> { Success = false, Error = ex.Message };
    }
  }

  public virtual async Task<RepositoryResult> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression)
  {
    var result = await _db.AnyAsync(expression);

    return result
      ? new RepositoryResult { Success = true }
      : new RepositoryResult { Success = false, Error = "Entity was not found." };
  }
}