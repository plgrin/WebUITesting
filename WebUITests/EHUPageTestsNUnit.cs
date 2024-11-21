using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;
using System.IO;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;


namespace WebUITests
{
    /// <summary>
    /// This class contains tests for verifying various functionalities on the EHU website.
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class EHUPageTestsNUnit
    {
        private static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>(() =>
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--incognito"); // Используем режим инкогнито для изоляции
            chromeOptions.AddArgument("--disable-extensions"); // Отключение расширений для стабильности
            return new ChromeDriver(chromeOptions);
        });

        /// <summary>
        /// Set up method that initializes the configuration and the Chrome WebDriver.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            driver.Value.Manage().Window.Maximize();

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        // Data provider for "VerifyNavigationToAboutEHUPage"
        private static IEnumerable<object[]> NavigationTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:AboutPageUrl"],
                "About",
                "About"
            };
        }

        /// <summary>
        /// Test to verify navigation to the "About EHU" page.
        /// </summary>
        [Test, TestCaseSource(nameof(NavigationTestCases))]
        public void VerifyNavigationToAboutEHUPage(string baseUrl, string aboutUrl, string expectedTitle, string expectedHeader)
        {
            driver.Value.Navigate().GoToUrl(baseUrl);

            var aboutLink = driver.Value.FindElement(By.LinkText("About"));
            aboutLink.Click();

            Assert.That(driver.Value.Url, Is.EqualTo(aboutUrl), "The URL does not match the expected value.");
            Assert.That(driver.Value.Title, Is.EqualTo(expectedTitle), "The page title does not match the expected value.");
            var header = driver.Value.FindElement(By.TagName("h1")).Text;
            Assert.That(header, Is.EqualTo(expectedHeader), "The content header does not match the expected value.");
        }

        // Data provider for "VerifySearchFunctionality"
        private static IEnumerable<object[]> SearchTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:SearchTerm"]
            };
        }

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Test, TestCaseSource(nameof(SearchTestCases))]
        public void VerifySearchFunctionality(string baseUrl, string searchTerm)
        {
            driver.Value.Navigate().GoToUrl(baseUrl);

            var searchButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();

            var searchBar = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(searchTerm);
            searchBar.SendKeys(Keys.Enter);

            Assert.That(driver.Value.Url, Does.Contain("/?s=" + searchTerm.Replace(" ", "+")), "The URL does not contain the expected search query.");

            var searchResults = driver.Value.FindElements(By.XPath("//*[@id=\"page\"]/div[3]")); 
            Assert.That(searchResults.Count, Is.GreaterThan(0), "No search results were found.");

            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains("study program", StringComparison.OrdinalIgnoreCase));
            Assert.That(resultsContainSearchTerm, Is.True, "Search results do not contain expected term 'study programs'.");
        }

        // Data provider for "VerifyLanguageChangeFunctionality"
        private static IEnumerable<object[]> LanguageChangeTestCases()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            yield return new object[]
            {
                config["TestSettings:EHUBaseUrl"],
                config["TestSettings:LithuanianVersionUrl"]
            };
        }

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Test, TestCaseSource(nameof(LanguageChangeTestCases))]
        public void VerifyLanguageChangeFunctionality(string baseUrl, string lithuanianUrl)
        {
            driver.Value.Navigate().GoToUrl(baseUrl);

            var languageSwitchButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();

            var ltButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();

            var wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Equals(lithuanianUrl));

            Assert.That(driver.Value.Url, Is.EqualTo(lithuanianUrl), "The URL does not match the expected value.");
        }

        /// <summary>
        /// Tear down method that closes the browser and cleans up the WebDriver instance.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            driver.Value.Quit();
        }
    }
}
