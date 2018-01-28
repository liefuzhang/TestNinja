using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests {
    [TestFixture]
    public class DemeritPointsCalculatorTests {
        [Test]
        public void CalculateDemeritPoints_SpeedIsZero_ReturnZero() {
            var calculator = new DemeritPointsCalculator();

            var result = calculator.CalculateDemeritPoints(0);

            Assert.That(result, Is.EqualTo(0));
        }


        [Test]
        public void CalculateDemeritPoints_SpeedIsUnderSpeedLimit_ReturnZero() {
            var calculator = new DemeritPointsCalculator();

            var result = calculator.CalculateDemeritPoints(50);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateDemeritPoints_SpeedIsAtSpeedLimit_ReturnZero() {
            var calculator = new DemeritPointsCalculator();

            var result = calculator.CalculateDemeritPoints(65);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateDemeritPoints_SpeedIsOverSpeedLimitAndUnderMaxSpeed_ReturnCorrectPoints() {
            var calculator = new DemeritPointsCalculator();

            var result = calculator.CalculateDemeritPoints(100);

            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]
        public void CalculateDemeritPoints_WhenCalled_ReturnDemeritPoints(int speed, int expectedResult) {
            var calculator = new DemeritPointsCalculator();

            var result = calculator.CalculateDemeritPoints(speed);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void CalculateDemeritPoints_SpeedIsUnderZeroOrOverMaxSpeed_ThrowArgumentOutOfRangeException(int speed) {
            var calculator = new DemeritPointsCalculator();

            Assert.That(() => calculator.CalculateDemeritPoints(speed), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
