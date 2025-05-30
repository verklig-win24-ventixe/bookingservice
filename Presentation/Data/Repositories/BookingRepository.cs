using System.Linq.Expressions;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Presentation.Data.Entities;
using Presentation.Interfaces;

namespace Presentation.Data.Repositories;

public class BookingRepository(DataContext context) : BaseRepository<BookingEntity>(context), IBookingRepository
{
  public override async Task<RepositoryResult<IEnumerable<BookingEntity>>> GetAllAsync()
  {
    try
    {
      var entities = await _db
        .Include(x => x.BookingOwner)
        .ThenInclude(x => x!.Address)
        .ToListAsync();

      return new RepositoryResult<IEnumerable<BookingEntity>> { Success = true, Result = entities };
    }
    catch (Exception ex)
    {
      return new RepositoryResult<IEnumerable<BookingEntity>> { Success = false, Error = ex.Message };
    }
  }

  public override async Task<RepositoryResult<BookingEntity?>> GetAsync(Expression<Func<BookingEntity, bool>> expression)
  {
    try
    {
      var entity = await _db
        .Include(x => x.BookingOwner)
        .ThenInclude(x => x!.Address)
        .FirstOrDefaultAsync(expression)
        ?? throw new Exception("Entity was not found.");

      return new RepositoryResult<BookingEntity?> { Success = true, Result = entity };
    }
    catch (Exception ex)
    {
      return new RepositoryResult<BookingEntity?> { Success = false, Error = ex.Message };
    }
  }
}