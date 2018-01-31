using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNinja.Mocking {
    public interface IBookingRepository {
        Booking GetOverlappingBooking(Booking booking);
    }

    public class BookingRepository : IBookingRepository {
        private IUnitOfWork _unitOfWork;

        public BookingRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public Booking GetOverlappingBooking(Booking booking) {
            var bookings = _unitOfWork.Query<Booking>()
                    .Where(b => b.Id != booking.Id && b.Status != "Cancelled");

            return bookings.FirstOrDefault(
                    b => booking.ArrivalDate >= b.ArrivalDate
                        && booking.ArrivalDate < b.DepartureDate
                        || booking.DepartureDate > b.ArrivalDate
                        && booking.DepartureDate <= b.DepartureDate);
        }
    }
}
