using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SeleniumExtensions
{
    public static class Extensions
    {
        #region WaitForElement Helper Methods
        //--------------------------------------------------------------------------------------------------------------------
        // An expectation for checking that an element is present on the DOM of a page and visible. 
        // Visibility means that the element is not only displayed but also has a height and width that is greater than 0.
        //--------------------------------------------------------------------------------------------------------------------
        public static void WaitForElementToBeVisible(this IWebDriver driver, By by, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForElements = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForElements.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException Element Info: {1}. " +
                            "\n --> Timed out after {2} seconds. \n URL:{3}", errorMessage, by, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // An expectation for checking that all elements present on the web page that match the locator.
        //--------------------------------------------------------------------------------------------------------------------
        public static void WaitForPresenceOfAllElements(this IWebDriver driver, By by, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForElements = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForElements.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException Element Info: {1}. " +
                            "\n --> Timed out after {2} seconds. \n URL:{3}", errorMessage, by, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // An expectation for checking that an element is present on the DOM of a page.This does not necessarily mean that the element is visible.
        //--------------------------------------------------------------------------------------------------------------------
        public static void WaitForElementExists(this IWebDriver driver, By by, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForElements = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForElements.Until(ExpectedConditions.ElementExists(by));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException Element Info: {1}. " +
                            "\n --> Timed out after {2} seconds. \n URL:{3}", errorMessage, by, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }
        #endregion

        #region WaitForUrl Helper Methods   
        public static void WaitForUrlContains(this IWebDriver driver, string UrlContent, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForUrl = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForUrl.Until(ExpectedConditions.UrlContains(UrlContent));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException URLInfo: {1}. " +
                        "\n --> Timed out after {2} seconds. \n URL:{3}", errorMessage, UrlContent, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }

        public static void WaitForUrlToBe(this IWebDriver driver, string UrlContent, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForUrl = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForUrl.Until(ExpectedConditions.UrlToBe(UrlContent));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException URLInfo: {1}.\n --> Timed out after {2} " +
                        "seconds. \n URL:{3}", errorMessage, UrlContent, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }
        #endregion

        #region WaitForTime Helper Methods
        public static void WaitForSeconds(this IWebDriver driver, int sleepTime)
        {
            if (sleepTime > 0)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(sleepTime));
            }
        }

        public static void WaitForMilliSeconds(this IWebDriver driver, int sleepTime)
        {
            if (sleepTime > 0)
            {
                System.Threading.Thread.Sleep(sleepTime);
            }
        }
        #endregion

        #region VerifyUrl Helper Methods 
        public static void VerifyUrlContains(this IWebDriver driver, string expectedUrl)
        {
            Assert.IsTrue(driver.Url.Contains(expectedUrl), "URL not verified(Contains). Expected Url:{0} Actual Url:{1}",
                expectedUrl, driver.Url);
        }

        public static void VerifyUrlEquals(this IWebDriver driver, string expectedUrl)
        {
            Assert.IsTrue(driver.Url.Equals(expectedUrl), "URL not verified(Equals). Expected Url:{0} Actual Url:{1}",
                expectedUrl, driver.Url);
        }

        public static void VerifyUrlStartsWith(this IWebDriver driver, string expectedUrl)
        {
            Assert.IsTrue(driver.Url.StartsWith(expectedUrl), "URL not verified(StartsWith). Expected Url:{0} Actual Url:{1}",
                expectedUrl, driver.Url);
        }

        public static void VerifyUrlEndsWith(this IWebDriver driver, string expectedUrl)
        {
            Assert.IsTrue(driver.Url.EndsWith(expectedUrl), "URL not verified(EndsWith). Expected Url:{0} Actual Url:{1}",
                expectedUrl, driver.Url);
        }
        #endregion

        #region Element Click Helper Methods 
        public static void ClickOn(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0, string errorMessage = "")
        {
            try
            {
                driver.WaitForElementToBeClickable(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                driver.FindElement(by).Click();
            }
            catch (StaleElementReferenceException)
            {
                driver.UseSeleniumToClick(by);
            }
            catch (ElementClickInterceptedException)
            {
                driver.UseJavascriptToClick(by);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }

        public static void ClickWithJavaScript(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            try
            {
                driver.WaitForElementToBeClickable(by, waitTime, errorMessagesEnabled, errorMessage);
                driver.WaitForSeconds(isStandbyTime);
                IWebElement el = driver.FindElement(by);
                driver.JavaScriptExecutor("arguments[0].click();", el);
            }
            catch (StaleElementReferenceException)
            {
                driver.UseJavascriptToClick(by);
            }
            catch (ElementClickInterceptedException)
            {
                driver.UseJavascriptToClick(by);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }
        #endregion

        #region SendKeys Helper Methods 

        public static void SendKeys(this IWebDriver driver, By by, string value, int waitTime = 5, int isStandbyTime = 0,
            string errorMessage = "")
        {
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                driver.FindElement(by).SendKeys(value);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }

        public static void ClearFirstThenSendKeys(this IWebDriver driver, By by, string value, int waitTime = 5,
            int clearFirstWaitTime = 0, string errorMessage = "")
        {
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(clearFirstWaitTime);
                driver.FindElement(by).Clear();
                driver.FindElement(by).SendKeys(value);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }
        #endregion

        public static string GetText(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            string getText = "";
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                getText = driver.FindElement(by).Text;
            }
            catch (NoSuchElementException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                       driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
            return getText;
        }

        public static bool HasElement(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> NoSuchElementException(HasElementV2) Element Info: {1}. \n URL:{2}",
                        errorMessage, by, driver.Url));
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public static bool IsDisplayed(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0)
        {
            bool getIsDisplayed;
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                getIsDisplayed = driver.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return getIsDisplayed;
        }
        public static void SelectByText(this IWebDriver driver, By by, string text, int waitTime = 5, int isStandbyTime = 0,
            string errorMessage = "")
        {
            try
            {
                driver.WaitForPresenceOfAllElements(by, waitTime, true, errorMessage);
                driver.WaitForSeconds(isStandbyTime);
                new SelectElement(driver.FindElement(by)).SelectByText(text);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }

        public static void SelectByValue(this IWebDriver driver, By by, string value, int waitTime = 5, int isStandbyTime = 0,
            string errorMessage = "")
        {
            try
            {
                driver.WaitForPresenceOfAllElements(by, waitTime, true, errorMessage);
                driver.WaitForSeconds(isStandbyTime);
                new SelectElement(driver.FindElement(by)).SelectByValue(value);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info: {1}. \n URL:{2}", errorMessage, by,
                    driver.Url));
            }
        }

        public static void SelectByIndex(this IWebDriver driver, By by, int index, int waitTime = 5, int isStandbyTime = 0,
            string errorMessage = "")
        {
            SelectElement element = new SelectElement(driver.FindElement(by));

            try
            {
                driver.WaitForPresenceOfAllElements(by, waitTime, true, errorMessage);
                driver.WaitForSeconds(isStandbyTime);
                element.SelectByIndex(index);
            }
            catch (NoSuchElementException)
            {
                try
                {
                    driver.WaitForSeconds(10);
                    element.SelectByIndex(index);
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info:{1}, Index:{2}. \n URL:{2}",
                        errorMessage, by, index, driver.Url));
                }
            }
        }

        public static bool CheckboxIsSelected(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0)
        {
            bool getCheckboxIsSelected;
            try
            {
                driver.WaitForElementToBeClickable(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                getCheckboxIsSelected = driver.FindElement(by).Selected;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return getCheckboxIsSelected;
        }

        public static string GetAttribute(this IWebDriver driver, By by, string getAttributeName, int waitTime = 5,
            int isStandbyTime = 0, bool errorMessagesEnabled = false, string errorMessage = "")
        {
            string getAttributeValue = "";
            try
            {
                driver.WaitForPresenceOfAllElements(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                getAttributeValue = driver.FindElement(by).GetAttribute(getAttributeName);
            }
            catch (NoSuchElementException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> NoSuchElementException Element Info:{1}. \n URL:{2}", errorMessage, by, 
                        driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
            return getAttributeValue;
        }

        public static int GetElementCount(this IWebDriver driver, By by, int waitTime = 5, int isStandbyTime = 0,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            int count = 0;
            try
            {
                driver.WaitForElementToBeVisible(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                count = driver.FindElements(by).Count;
            }
            catch (NoSuchElementException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> NoSuchElementException(GetElementCountV2) Element Info: {1}. \n URL:{2}",
                        errorMessage, by, driver.Url));
                }
                else
                {
                    return count;
                }
            }
            return count;
        }

        public static void GoToUrl(this IWebDriver driver, string Url)
        {
            driver.Navigate().GoToUrl(Url);
        }

        public static void Refresh(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        public static bool IsNullOrEmpty(this IWebDriver driver, string text)
        {
            if (text == "")
            {
                return true;
            }

            else if (text == null)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public static bool HasValue(this IWebDriver driver, string text)
        {
            if (text == "")
            {
                return false;
            }

            else if (text == null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public static void JavaScriptExecutor(this IWebDriver driver, string script, int isStandbyTime = 0)
        {
            driver.WaitForSeconds(isStandbyTime);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(script);
        }

        public static void JavaScriptExecutor(this IWebDriver driver, string script, By by, int isStandbyTime = 0)
        {
            driver.WaitForSeconds(isStandbyTime);
            IWebElement el = driver.FindElement(by);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(script, el);
        }

        public static void JavaScriptExecutor(this IWebDriver driver, string script, IWebElement element, int isStandbyTime = 0)
        {
            driver.WaitForSeconds(isStandbyTime);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(script, element);
        }

        public static void HardRefreshWithJavaScript(this IWebDriver driver)
        {
            driver.JavaScriptExecutor("location.reload(true);");
        }

        #region private Helper Methods
        private static void WaitForElementToBeClickable(this IWebDriver driver, By by, int waitTime,
            bool errorMessagesEnabled = false, string errorMessage = "")
        {
            WebDriverWait waitForElements = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            try
            {
                waitForElements.Until(ExpectedConditions.ElementToBeClickable(by));
            }
            catch (WebDriverTimeoutException)
            {
                if (errorMessagesEnabled)
                {
                    Assert.Fail(String.Format("{0} \n>> WebDriverTimeoutException Element Info:{1}. " +
                            "\n --> Timed out after {2} seconds. \n URL:{3}", errorMessage, by, waitTime.ToString(), driver.Url));
                }
                else
                {
                    driver.WaitForMilliSeconds(10);
                }
            }
        }

        private static void UseJavascriptToClick(this IWebDriver driver, By by, int waitTime = 10, int isStandbyTime = 2)
        {
            try
            {
                driver.WaitForElementToBeClickable(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                IWebElement el = driver.FindElement(by);
                driver.JavaScriptExecutor("arguments[0].click();", el);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format(">> NoSuchElementException Element Info: {0}. \n URL:{1}", by, driver.Url));
            }
        }

        private static void UseSeleniumToClick(this IWebDriver driver, By by, int waitTime = 10, int isStandbyTime = 2)
        {
            try
            {
                driver.WaitForElementToBeClickable(by, waitTime);
                driver.WaitForSeconds(isStandbyTime);
                IWebElement el = driver.FindElement(by);
                el.Click();
            }
            catch (NoSuchElementException)
            {
                Assert.Fail(String.Format(">> NoSuchElementException Element Info: {0}. \n URL:{1}", by, driver.Url));
            }
        }
        #endregion
    }
}