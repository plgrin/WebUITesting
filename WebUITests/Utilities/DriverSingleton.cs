using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Utilities
{
    public class DriverSingleton
    {
        private static ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>(() =>
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--incognito");
            chromeOptions.AddArgument("--disable-extensions");
            var driver = new ChromeDriver(chromeOptions); new ChromeDriver(chromeOptions);

            driver.Manage().Window.Maximize();
            return driver;
        });

        private DriverSingleton() { }

        public static IWebDriver Driver
        {
            get
            {
                if (!_driver.IsValueCreated)
                {
                    throw new InvalidOperationException("Driver is not initialized.");
                }
                return _driver.Value;
            }
        }

        public static void InitializeDriver()
        {
            if (!_driver.IsValueCreated)
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--incognito");
                chromeOptions.AddArgument("--disable-extensions");
                _driver.Value = new ChromeDriver(chromeOptions);

                _driver.Value.Manage().Window.Maximize();
            }
        }

        public static void QuitDriver()
        {
            if (_driver.IsValueCreated)
            {
                _driver.Value.Quit();
                _driver.Dispose();
            }
        }
    }
}
