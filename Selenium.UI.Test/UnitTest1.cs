using OpenQA.Selenium;
using SeleniumExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Selenium.UI.Test
{
    [TestClass]
    [DeploymentItem("drivers", "")]
    public class UnitTest1
    {
        IWebDriver driver = null;

        [TestInitialize]
        public void Init()
        {
            driver = DriverUtility.CreateWebDriver();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            driver?.Quit();
        }

        [TestMethod]
        public void TestMethod1()
        {
            driver.GoToUrl("https://www.google.com.tr");
        }
    }
}
