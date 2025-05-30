using Presentation.Models;

namespace Presentation.Interfaces;

public interface IBookingService
{
  Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
}
