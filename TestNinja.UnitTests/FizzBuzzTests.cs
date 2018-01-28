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
    class FizzBuzzTests {
        [Test]
        [TestCase(0, "FizzBuzz")]
        [TestCase(1, "1")]
        [TestCase(3, "Fizz")]
        [TestCase(4, "4")]
        [TestCase(5, "Buzz")]
        [TestCase(15, "FizzBuzz")]
        [TestCase(30, "FizzBuzz")]
        public void GetOutput_WhenCalled_ReturnString(int num, string expectedResult) {

            var result = FizzBuzz.GetOutput(num);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        
    }
}

