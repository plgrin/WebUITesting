using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace  WebUITests_Xunit.Utilities
{
    public static class DriverManager
    {
        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--disable-extensions");

            var driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
