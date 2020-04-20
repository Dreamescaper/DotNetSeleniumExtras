using NUnit.Framework;

// Tests are targeted specifically at equality 
#pragma warning disable NUnit2010 // Use EqualConstraint.

namespace SeleniumExtras.PageObjects
{
    [TestFixture]
    public class FindsByAttributeTests
    {
        [Test]
        public void TestEquality()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Test" };
            FindsByAttribute second = new FindsByAttribute() { How = How.Id, Using = "Test" };
            Assert.That(first.Equals(second), Is.True);
            Assert.That(object.ReferenceEquals(first, second), Is.False);
            Assert.That(first == second, Is.True);
            Assert.That(first != second, Is.False);
        }

        [Test]
        public void TestSameInstanceEquality()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Test" };
            FindsByAttribute second = first;
            Assert.That(first == second, Is.True);
            Assert.That(second == first, Is.True);
            Assert.That(first.Equals(second), Is.True);
            Assert.That(second.Equals(first), Is.True);
            Assert.That(object.ReferenceEquals(first, second), Is.True);
        }

        [Test]
        public void TestInequalityOfUsing()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Hello" };
            FindsByAttribute second = new FindsByAttribute() { How = How.Id, Using = "World" };
            Assert.That(first.Equals(second), Is.False);
            Assert.That(first == second, Is.False);
            Assert.That(first != second, Is.True);
        }

        [Test]
        public void TestInequalityOfHow()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Name, Using = "Test" };
            FindsByAttribute second = new FindsByAttribute() { How = How.Id, Using = "Test" };
            Assert.That(first.Equals(second), Is.False);
            Assert.That(first == second, Is.False);
            Assert.That(first != second, Is.True);
        }

        [Test]
        public void TestInequalityOfPriority()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Test", Priority = 1 };
            FindsByAttribute second = new FindsByAttribute() { How = How.Id, Using = "Test", Priority = 2 };
            Assert.That(first.Equals(second), Is.False);
            Assert.That(first == second, Is.False);
            Assert.That(first != second, Is.True);
        }

        [Test]
        public void TestInequalityOfNull()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Test" };
            FindsByAttribute second = null;
            Assert.That(first.Equals(second), Is.False);

            // Must test order of arguments for overridden operators
            Assert.That(first == second, Is.False);
            Assert.That(first != second, Is.True);
            Assert.That(second == first, Is.False);
            Assert.That(second != first, Is.True);
        }

        [Test]
        public void TestEqualityOfTwoNullInstances()
        {
            FindsByAttribute first = null;
            FindsByAttribute second = null;

            // Must test order of arguments for overridden operators
            Assert.That(first == second, Is.True);
            Assert.That(first != second, Is.False);
            Assert.That(second == first, Is.True);
            Assert.That(second != first, Is.False);
        }

        [Test]
        public void TestComparison()
        {
            FindsByAttribute first = new FindsByAttribute() { How = How.Id, Using = "Test", Priority = 1 };
            FindsByAttribute second = new FindsByAttribute() { How = How.Id, Using = "Test", Priority = 2 };
            Assert.Less(first, second);
            Assert.Greater(second, first);
        }
    }
}
