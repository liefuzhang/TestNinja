using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    public class BookingHelperTests_OverlappingBookingsExist {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBookingRepository> _bookingRepository;
        private IQueryable<Booking> _bookings;
        private Booking _existingBooking1;
        private Booking _existingBooking2;

        [SetUp]
        public void Setup() {
            _unitOfWork = new Mock<IUnitOfWork>();
            _bookingRepository = new Mock<IBookingRepository>();
            _unitOfWork.Object.BookingRepository = _bookingRepository.Object;
            BookingHelper.UnitOfWork = _unitOfWork.Object;
            _existingBooking1 = new Booking {
                Reference = "b",
                ArrivalDate = ArriveOn(2017, 2, 11),
                DepartureDate = DepartAt(2017, 2, 13)
            };
            _existingBooking2 = new Booking {
                Reference = "c",
                ArrivalDate = ArriveOn(2017, 3, 11),
                DepartureDate = DepartAt(2017, 3, 13)
            };
            _bookings = new List<Booking> { _existingBooking1, _existingBooking2 }.AsQueryable();
        }

        [Test]
        public void BookingStartsAndFinishesBeforeExistingBookings_ReturnEmptyString() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking1.ArrivalDate, days: 2),
                DepartureDate = Before(_existingBooking1.ArrivalDate)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingReference() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking1.ArrivalDate),
                DepartureDate = After(_existingBooking1.ArrivalDate)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(_existingBooking1.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingReference() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking1.ArrivalDate),
                DepartureDate = After(_existingBooking1.DepartureDate)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(_existingBooking1.Reference));
        }


        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfAnExistingBookingFinishes_ReturnExistingBookingReference() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking1.ArrivalDate),
                DepartureDate = Before(_existingBooking1.DepartureDate)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(_existingBooking1.Reference));
        }

        [Test]
        public void BookingStartsInTheMiddleOfAndFinishesAfterAnExistingBookingFinishes_ReturnExistingBookingReference() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking1.ArrivalDate),
                DepartureDate = After(_existingBooking1.DepartureDate)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(_existingBooking1.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBookingFinishes_ReturnEmptyString() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = After(_existingBooking1.DepartureDate),
                DepartureDate = After(_existingBooking1.DepartureDate, 2)
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void BookingsOverlapButNewBookingIsCancelled_ReturnEmptyString() {
            var booking = new Booking {
                Id = 1,
                ArrivalDate = Before(_existingBooking1.ArrivalDate),
                DepartureDate = After(_existingBooking1.ArrivalDate),
                Status = "Cancelled"
            };

            _unitOfWork.Setup(u => u.BookingRepository.GetActiveBookings(booking.Id)).Returns(_bookings);

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(""));
        }


        // helper methods
        private DateTime ArriveOn(int year, int month, int day) {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartAt(int year, int month, int day) {
            return new DateTime(year, month, day, 10, 0, 0);
        }

        private DateTime Before(DateTime dateTime, int days = 1) {
            return dateTime.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1) {
            return dateTime.AddDays(days);
        }
    }
}
