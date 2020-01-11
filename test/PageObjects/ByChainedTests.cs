using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Is = NUnit.Framework.Is;

namespace SeleniumExtras.PageObjects
{
    [TestFixture]
    public class ByChainedTests
    {
        private readonly By by = By.Name("cheese");
        private readonly By by2 = By.Name("photo");

        [Test]
        public void FindElementZeroBy()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            ByChained byChained = new ByChained();
            Assert.Throws<NoSuchElementException>(() => byChained.FindElement(driver.Object));
        }

        [Test]
        public void FindElementsZeroBy()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            ByChained byChained = new ByChained();

            Assert.That(byChained.FindElements(driver.Object), Is.EqualTo(new List<IWebElement>().AsReadOnly()));
        }

        [Test]
        public void FindElementOneBy()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();
            Mock<IAllElement> elem1 = new Mock<IAllElement>();
            Mock<IAllElement> elem2 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement>() { elem1.Object, elem2.Object }.AsReadOnly();
            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);

            ByChained byChained = new ByChained(by);
            Assert.AreEqual(byChained.FindElement(driver.Object), elem1.Object);
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
        }

        [Test]
        public void FindElementsOneBy()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();
            Mock<IAllElement> elem1 = new Mock<IAllElement>();
            Mock<IAllElement> elem2 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement>() { elem1.Object, elem2.Object }.AsReadOnly();
            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);

            ByChained byChained = new ByChained(by);
            Assert.AreEqual(byChained.FindElements(driver.Object), elems12);
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
        }

        [Test]
        public void FindElementOneByEmpty()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();
            var elems = new List<IWebElement>().AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems);

            ByChained byChained = new ByChained(by);
            try
            {
                byChained.FindElement(driver.Object);
                Assert.Fail("Expected NoSuchElementException!");
            }
            catch (NoSuchElementException)
            {
                driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
                Assert.Pass();
            }
        }

        [Test]
        public void FindElementsOneByEmpty()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();
            var elems = new List<IWebElement>().AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems);

            ByChained byChained = new ByChained(by);

            Assert.That(byChained.FindElements(driver.Object), Is.EqualTo(elems));
        }

        [Test]
        public void FindElementTwoBy()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            Mock<IAllElement> elem1 = new Mock<IAllElement>();
            Mock<IAllElement> elem2 = new Mock<IAllElement>();
            Mock<IAllElement> elem3 = new Mock<IAllElement>();
            Mock<IAllElement> elem4 = new Mock<IAllElement>();
            Mock<IAllElement> elem5 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement>() { elem1.Object, elem2.Object }.AsReadOnly();
            var elems34 = new List<IWebElement>() { elem3.Object, elem4.Object }.AsReadOnly();
            var elems5 = new List<IWebElement>() { elem5.Object }.AsReadOnly();
            var elems345 = new List<IWebElement>() { elem3.Object, elem4.Object, elem5.Object }.AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            elem1.Setup(_ => _.FindElements(by2)).Returns(elems34);
            elem2.Setup(_ => _.FindElements(by2)).Returns(elems5);

            ByChained byChained = new ByChained(by, by2);
            Assert.That(byChained.FindElement(driver.Object), Is.EqualTo(elem3.Object));
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
            elem1.Verify(_ => _.FindElements(by2), Times.Once);
            elem2.Verify(_ => _.FindElements(by2), Times.Once);
        }

        [Test]
        public void FindElementTwoByEmptyParent()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            var elems = new List<IWebElement>().AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems);

            ByChained byChained = new ByChained(by, by2);
            try
            {
                byChained.FindElement(driver.Object);
                Assert.Fail("Expected NoSuchElementException!");
            }
            catch (NoSuchElementException)
            {
                driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
                Assert.Pass();
            }
        }

        [Test]
        public void FindElementsTwoByEmptyParent()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            var elems = new List<IWebElement>().AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems);

            ByChained byChained = new ByChained(by, by2);

            Assert.That(byChained.FindElements(driver.Object), Is.EqualTo(elems));
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
        }

        [Test]
        public void FindElementTwoByEmptyChild()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            Mock<IAllElement> elem1 = new Mock<IAllElement>();
            Mock<IAllElement> elem2 = new Mock<IAllElement>();
            Mock<IAllElement> elem5 = new Mock<IAllElement>();

            var elems = new List<IWebElement>().AsReadOnly();
            var elems12 = new List<IWebElement>() { elem1.Object, elem2.Object }.AsReadOnly();
            var elems5 = new List<IWebElement>() { elem5.Object }.AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            elem1.Setup(_ => _.FindElements(by2)).Returns(elems);
            elem2.Setup(_ => _.FindElements(by2)).Returns(elems5);

            ByChained byChained = new ByChained(by, by2);
            Assert.That(byChained.FindElement(driver.Object), Is.EqualTo(elem5.Object));
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
            elem1.Verify(_ => _.FindElements(by2), Times.Once);
            elem2.Verify(_ => _.FindElements(by2), Times.Once);
        }

        [Test]
        public void FindElementsTwoByEmptyChild()
        {
            Mock<IAllDriver> driver = new Mock<IAllDriver>();

            Mock<IAllElement> elem1 = new Mock<IAllElement>();
            Mock<IAllElement> elem2 = new Mock<IAllElement>();
            Mock<IAllElement> elem3 = new Mock<IAllElement>();
            Mock<IAllElement> elem4 = new Mock<IAllElement>();
            Mock<IAllElement> elem5 = new Mock<IAllElement>();
            var elems = new List<IWebElement>().AsReadOnly();
            var elems12 = new List<IWebElement>() { elem1.Object, elem2.Object }.AsReadOnly();
            var elems34 = new List<IWebElement>() { elem3.Object, elem4.Object }.AsReadOnly();
            var elems5 = new List<IWebElement>() { elem5.Object }.AsReadOnly();
            var elems345 = new List<IWebElement>() { elem3.Object, elem4.Object, elem5.Object }.AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            elem1.Setup(_ => _.FindElements(by2)).Returns(elems);
            elem2.Setup(_ => _.FindElements(by2)).Returns(elems5);

            ByChained byChained = new ByChained(by, by2);
            Assert.That(byChained.FindElements(driver.Object), Is.EqualTo(new[] { elem5.Object }));
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.Once);
            elem1.Verify(_ => _.FindElements(by2), Times.Once);
            elem2.Verify(_ => _.FindElements(by2), Times.Once);
        }

        [Test]
        public void TestEquals()
        {
            Assert.That(new ByChained(By.Id("cheese"), by2),
                Is.EqualTo(new ByChained(By.Id("cheese"), by2)));
        }
    }
}
