using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Stack = TestNinja.Fundamentals.Stack<string>;

namespace TestNinja.UnitTests {
    [TestFixture]
    public class StackTests {
        [Test]
        public void Push_ArgIsNull_ThrowArgumentNullException() {
            var stack = new Stack();

            Assert.That(()=> stack.Push(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Push_ValidArg_AddTheObjectToTheStack() {
            var stack = new Stack();

            stack.Push("a");

            Assert.That(stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_EmptyStack_ReturnZero() {
            var stack = new Stack();

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Pop_StackWithAFewObjects_ReturnObjectOnTheTop() {
            var stack = new Stack();
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            var result = stack.Pop();

            Assert.That(result, Is.EqualTo("c"));
        }

        [Test]
        public void Pop_StackWithAFewObjects_RemoveObjectOnTheTop() {
            var stack = new Stack();
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            var result = stack.Pop();

            Assert.That(stack.Count, Is.EqualTo(2));
        }

        [Test]
        public void Pop_EmptyStack_ThrowInvalidOperationException() {
            var stack = new Stack();


            Assert.That(()=> stack.Pop(), Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Peek_EmptyStack_ThrowInvalidOperationException() {
            var stack = new Stack();


            Assert.That(() => stack.Peek(), Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Peek_StackWithAFewObjects_ReturnObjectOnTheTop() {
            var stack = new Stack();
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            var result = stack.Peek();

            Assert.That(result, Is.EqualTo("c"));
        }

        [Test]
        public void Peek_StackWithAFewObjects_DoesNotRemoveObjectOnTheTop() {
            var stack = new Stack();
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");

            var result = stack.Peek();

            Assert.That(stack.Count, Is.EqualTo(3));
        }

    }
}
