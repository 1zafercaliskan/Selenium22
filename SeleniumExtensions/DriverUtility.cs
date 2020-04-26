using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumExtensions
{
    class DriverUtility
    {
        private static ChromeOptions options = new ChromeOptions();

        static DriverUtility()
        {
            options.AddArgument("--incognito");
            options.AddArgument("--lang=en-US");
            options.AddArgument("--disk-cache-size=0");
            options.AddArgument("--window-size=1600,900");
            options.AddArgument("--allow-running-insecure-content");
        }

        private static IWebDriver CreateChromeDriver()
        {
            options.AcceptInsecureCertificates = true;
            return new ChromeDriver(options);
        }

        private static IWebDriver CreateChromeHeadlessDriver()
        {
            options.AcceptInsecureCertificates = true;
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            return new ChromeDriver(options);
        }

        public static IWebDriver CreateWebDriver()
        {
            IWebDriver driver = null;

#if DEBUG
            driver = CreateChromeDriver();
#else
            driver = CreateChromeHeadlessDriver();
#endif
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return driver;
        }
    }
}

//https://www.chromium.org/developers/how-tos/run-chromium-with-flags
//--headless Run in headless mode, i.e., without a UI or display server dependencies.
//--disable-gpu Disables GPU hardware acceleration. If software renderer is not in place, then the GPU process won't launch.
//--window-size=1600,900 Sets the initial window size.Provided as string in the format "800,600". 
//--disk-cache-size=0 Forces the maximum disk space to be used by the disk cache, in bytes.
//--incognito Causes the browser to launch directly in incognito mode.
//--allow-running-insecure-content By default, an https page cannot run JavaScript, CSS or plugins from http URLs.
//-> This provides an override to get the old insecure behavior.
//--disable-local-storage Disable LocalStorage.
//--allow-insecure-localhost Enables TLS/SSL errors on localhost to be ignored (no interstitial, no blocking of requests). 
//--disable-extensions Disable extensions. 