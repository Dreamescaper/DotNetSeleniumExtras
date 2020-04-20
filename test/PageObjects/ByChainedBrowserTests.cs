using OpenQA.Selenium;
using NUnit.Framework;
using SeleniumExtras.Environment;

namespace SeleniumExtras.PageObjects
{
    [TestFixture]
    public class ByChainedBrowserTests : DriverTestFixture
    {
        //TODO: Move these to a standalone class when more tests rely on the server being up
        [OneTimeSetUp]
        public void RunBeforeAnyTest()
        {
            EnvironmentManager.Instance.WebServer.Start();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            EnvironmentManager.Instance.WebServer.Stop();
        }

        [Test]        
        public void FindElementNotFound_ShouldThrow()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("NotFoundAtAll"));
            try
            {
                by.FindElement(driver);
                Assert.Fail("Expected NotFoundExcepotion");
            }
            catch (NotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void FindElementsNotFound_ShouldBeZero()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("NotFoundAtAll"));
                
            Assert.That(by.FindElements(driver).Count, Is.EqualTo(0));
        }

        [Test]
        public void FindElement_ShouldReturnOne()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("div1"));
            Assert.That(by.FindElement(driver).Displayed, Is.True);
        }

        [Test]
        public void FindElements_ShouldReturnMultiple()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("div1"));
            Assert.That(by.FindElements(driver).Count, Is.EqualTo(4));
        }

        [Test]
        public void FindElement_TwoBys_ShouldReturnOne()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("classes"), By.CssSelector(".one"));
            Assert.That(by.FindElement(driver).Text, Is.EqualTo("Find me"));
        }

        [Test]
        public void FindElements_TwoBys_ShouldReturnMultiple()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("classes"), By.CssSelector(".one"));
            Assert.That(by.FindElements(driver).Count, Is.EqualTo(2));
        }

        [Test]
        public void FindElements_TwoBys_ShouldReturnZero()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            var by = new ByChained(By.Name("classes"), By.CssSelector(".NotFound"));
            Assert.That(by.FindElements(driver).Count, Is.EqualTo(0));
        }

        [Test]
        public void FindElement_TwoBys_ShouldThrow()
        {
            driver.Url = nestedPage;
            driver.Navigate();

            try
            {
                var by = new ByChained(By.Name("classes"), By.CssSelector(".NotFound"));
                by.FindElement(driver);
                Assert.Fail("Expected NotFoundException");
            }
            catch (NotFoundException) 
            {
                Assert.Pass();
            }
        }
    }
}
