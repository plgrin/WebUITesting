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
        private string? ehuBaseUrl;
        private string? aboutPageUrl;
        private string? searchTerm;
        private string? lithuanianVersionUrl;

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

            // Retrieve data from the configuration file
            ehuBaseUrl = configuration["TestSettings:EHUBaseUrl"];
            aboutPageUrl = configuration["TestSettings:AboutPageUrl"];
            searchTerm = configuration["TestSettings:SearchTerm"];
            lithuanianVersionUrl = configuration["TestSettings:LithuanianVersionUrl"];
        }

        /// <summary>
        /// Test to verify navigation to the "About EHU" page.
        /// </summary>
        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            driver.Value.Navigate().GoToUrl(ehuBaseUrl);
            var aboutLink = driver.Value.FindElement(By.LinkText("About"));
            aboutLink.Click();
            Assert.That(driver.Value.Url, Is.EqualTo(aboutPageUrl), "The URL does not match the expected value.");
            Assert.That(driver.Value.Title, Is.EqualTo("About"), "The page title does not match the expected value.");
            var header = driver.Value.FindElement(By.TagName("h1")).Text;
            Assert.That(header, Is.EqualTo("About"), "The content header does not match the expected value.");
        }

        /// <summary>
        /// Test to verify the search functionality on the EHU website.
        /// </summary>
        [Test]
        public void VerifySearchFunctionality()
        {
            // Step 1: Navigate to the base URL
            driver.Value.Navigate().GoToUrl(ehuBaseUrl);

            // Step 2: Locate and click the search button to open the search bar
            var searchButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();

            // Step 3: Locate the search bar and input the search term
            var searchBar = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(searchTerm);

            // Step 4: Submit the search query
            searchBar.SendKeys(Keys.Enter);

            // Step 5: Verify the URL contains the search term
            Assert.That(driver.Value.Url, Does.Contain("/?s=study+programs"), "The URL does not contain the expected search query.");

            // Step 6: Verify search results are displayed
            var searchResults = driver.Value.FindElements(By.XPath("//*[@id=\"page\"]/div[3]")); 
            Assert.That(searchResults.Count, Is.GreaterThan(0), "No search results were found.");

            // Step 7 (Optional): Check if search results contain relevant content
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains("study program", StringComparison.OrdinalIgnoreCase));
            Assert.That(resultsContainSearchTerm, Is.True, "Search results do not contain expected term 'study programs'.");
        }

        /// <summary>
        /// Test to verify the functionality of changing the website language from English to Lithuanian.
        /// </summary>
        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Value.Navigate().GoToUrl(ehuBaseUrl);

            // Step 2: Locate and click the language switcher to open the menu
            var languageSwitchButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();

            // Step 3: Locate and click the Lithuanian language option (Lietuvių)
            var ltButton = driver.Value.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();

            // Step 4: Wait for the Lithuanian version to load and verify the URL
            var wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Equals(lithuanianVersionUrl));

            // Step 5: Assert the URL has changed to the Lithuanian version
            Assert.That(driver.Value.Url, Is.EqualTo(lithuanianVersionUrl), "The URL does not match the expected value.");
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
