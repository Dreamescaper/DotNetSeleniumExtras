/*
Copyright 2015 Software Freedom Conservancy
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

Unless required byAll applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */

using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Is = NUnit.Framework.Is;

namespace SeleniumExtras.PageObjects
{
    [TestFixture]
    public class ByAllTests
    {
        private readonly By by = By.Name("cheese");
        private readonly By by2 = By.Name("photo");

        [Test]
        public void FindElementZeroBy()
        {
            var driver = new Mock<IAllDriver>();

            var byAll = new ByAll();

            Assert.Throws<NoSuchElementException>(() => byAll.FindElement(driver.Object));
            Assert.That(byAll.FindElements(driver.Object), Is.EqualTo(new List<IWebElement>().AsReadOnly()));
        }

        [Test]
        public void FindElementOneBy()
        {
            var driver = new Mock<IAllDriver>();
            var elem1 = new Mock<IAllElement>();
            var elem2 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement> { elem1.Object, elem2.Object }.AsReadOnly();
            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            var byAll = new ByAll(by);

            // findElement
            Assert.That(elem1.Object, Is.EqualTo(byAll.FindElement(driver.Object)));
            //findElements
            Assert.That(byAll.FindElements(driver.Object), Is.EqualTo(elems12));

            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.AtLeastOnce);
        }

        [Test]
        public void FindElementOneByEmpty()
        {
            var driver = new Mock<IAllDriver>();
            var empty = new List<IWebElement>().AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(empty);

            var byAll = new ByAll(by);

            // one element
            Assert.Throws<NoSuchElementException>(() => byAll.FindElement(driver.Object));
            Assert.That(byAll.FindElements(driver.Object), Is.EqualTo(empty));

            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.AtLeastOnce);
        }

        [Test]
        public void FindElementTwoBy()
        {
            var driver = new Mock<IAllDriver>();

            var elem1 = new Mock<IAllElement>();
            var elem2 = new Mock<IAllElement>();
            var elem3 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement> { elem1.Object, elem2.Object }.AsReadOnly();
            var elems23 = new List<IWebElement> { elem2.Object, elem3.Object }.AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            driver.Setup(_ => _.FindElements(by2.Mechanism, by2.Criteria)).Returns(elems23);

            var byAll = new ByAll(by, by2);

            // findElement
            Assert.That(byAll.FindElement(driver.Object), Is.EqualTo(elem2.Object));

            //findElements
            var result = byAll.FindElements(driver.Object);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(elem2.Object));

            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.AtLeastOnce);
            driver.Verify(_ => _.FindElements(by2.Mechanism, by2.Criteria), Times.AtLeastOnce);
        }

        [Test]
        public void FindElementDisjunct()
        {
            var driver = new Mock<IAllDriver>();

            var elem1 = new Mock<IAllElement>();
            var elem2 = new Mock<IAllElement>();
            var elem3 = new Mock<IAllElement>();
            var elem4 = new Mock<IAllElement>();
            var elems12 = new List<IWebElement> { elem1.Object, elem2.Object }.AsReadOnly();
            var elems34 = new List<IWebElement> { elem3.Object, elem4.Object }.AsReadOnly();

            driver.Setup(_ => _.FindElements(by.Mechanism, by.Criteria)).Returns(elems12);
            driver.Setup(_ => _.FindElements(by2.Mechanism, by2.Criteria)).Returns(elems34);

            var byAll = new ByAll(by, by2);

            Assert.Throws<NoSuchElementException>(() => byAll.FindElement(driver.Object));

            var result = byAll.FindElements(driver.Object);
            Assert.That(result.Count, Is.EqualTo(0));
            driver.Verify(_ => _.FindElements(by.Mechanism, by.Criteria), Times.AtLeastOnce);
            driver.Verify(_ => _.FindElements(by2.Mechanism, by2.Criteria), Times.AtLeastOnce);
        }

    }
}
