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
    public class BookingHelperTests {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IBookingRepository> _bookingRepository;

        [SetUp]
        public void Setup() {
            _unitOfWork = new Mock<IUnitOfWork>();
            _bookingRepository = new Mock<IBookingRepository>();
            _unitOfWork.Object.BookingRepository = _bookingRepository.Object;
            BookingHelper.UnitOfWork = _unitOfWork.Object;
        }

        [Test]
        public void OverlappingBookingsExist_BookingStatusIsCancelled_ReturnEmptyString() {
            var booking = new Booking { Status = "Cancelled" };

            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void OverlappingBookingsExist_NoOverlappingBooking_ReturnEmptyString() {
            var booking = new Booking();

            _bookingRepository.Setup(br => br.GetOverlappingBooking(booking)).Returns((Booking) null);
            
            var result = BookingHelper.OverlappingBookingsExist(booking);

            Assert.That(result, Is.EqualTo(""));
        }

    }
}
