using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking {
    public static class BookingHelper {
        public static IUnitOfWork UnitOfWork;

        static BookingHelper() {
            UnitOfWork = new UnitOfWork();
        }

        public static string OverlappingBookingsExist(Booking booking) {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var overlappingBooking = UnitOfWork.BookingRepository.GetOverlappingBooking(booking);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }

    public interface IUnitOfWork {
        IQueryable<T> Query<T>();
        IBookingRepository BookingRepository { get; set; }
    }

    public class UnitOfWork : IUnitOfWork {
        public UnitOfWork() {
            BookingRepository = new BookingRepository(this);
        }
        public IBookingRepository BookingRepository { get; set; }

        public IQueryable<T> Query<T>() {
            return new List<T>().AsQueryable();
        }

    }

    public class Booking {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}