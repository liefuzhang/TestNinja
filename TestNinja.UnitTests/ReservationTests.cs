using System;
using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace TestNinja.UnitTests {
    [TestFixture]
    public class ReservationTests {
        [Test]
        public void CanBeCancelledBy_AdminCancelling_ReturnsTrue() {
            // Arrange
            var reservation = new Reservation();

            // Act
            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

            // Assert
            Assert.That(result == true);
        }

        [Test]
        public void CanBeCancelledBy_SameUserCancellingReservation_ReturnsTrue() {
            // Arrange
            var user = new User();
            var reservation = new Reservation { MadeBy = user };


            // Act
            var result = reservation.CanBeCancelledBy(user);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanBeCancelledBy_AnotherUserCancellingReservation_ReturnsFalse() {
            // Arrange
            var reservation = new Reservation {MadeBy = new User()};


            // Act
            var result = reservation.CanBeCancelledBy(new User());

            // Assert
            Assert.IsFalse(result);
        }

    }
}
