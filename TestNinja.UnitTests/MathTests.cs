﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using TestNinja.Fundamentals;
using Math = TestNinja.Fundamentals.Math;

namespace TestNinja.UnitTests {
    [TestFixture]
    public class MathTests {
        private Math _math;
        // Setup
        [SetUp]
        public void Setup() {
            _math = new Math();
        }

        [Test]
        public void Add_WhenCalled_ReturnTheSumOfArguments() {
            var result = _math.Add(1, 2);

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 1, 1)]
        public void Max_WhenCalled_ReturnTheGreaterArgument(int a, int b, int expectedResult) {

            var result = _math.Max(a, b);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
